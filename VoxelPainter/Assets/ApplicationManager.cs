using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AppData
{
    public string Id;
    public List<Block.BlockData> m_Blocks = new List<Block.BlockData>();
    public Vector3 Center;

    public Texture2D IconTexture2D;
}

public class ApplicationManager : Singleton<ApplicationManager>
{
    public AppData m_AppData;

    public Camera ScreenshotCamera;
    public RenderTexture ScreenshotRenderTexture;

    private string SaveDataPath = "/SaveData/";
    private string SymbolPath = "/Presets/";

    private string SaveDataFileName = "SaveData.json";

    public Block blockPrefab;

    // Move to symbols
    public List<AppData> Presets;
    private Dictionary<string, AppData> _presetLookUp = new Dictionary<string, AppData>();

    private void Awake()
    {
        var rootPath = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
        SaveDataPath = rootPath + SaveDataPath;
        SymbolPath = rootPath + SymbolPath;

        Directory.CreateDirectory(SaveDataPath);
        Directory.CreateDirectory(SymbolPath);

        SaveDataPath += SaveDataFileName;

        LoadPresets();
    }

    public void SaveSymbol(List<Block> blocks)
    {
        if (blocks.Count == 0)
            return;

        var data = new AppData();

        data.Id = Guid.NewGuid().ToString();

        foreach (var block in blocks)
        {
            block.Save();
            data.m_Blocks.Add(block.mBlockData);
        }

        var transforms = blocks.Select(x => x.transform).ToArray();
        var center = TransformUtils.Center(transforms);

        data.Center = center;

        foreach (var block in blocks)
        {
            foreach (var f in block.faces)
                f.Deselect();
        }

        var xMax = TransformUtils.XMax(transforms);
        var zMax = TransformUtils.ZMax(transforms);

        var xDist = (xMax - center.x) * 2;
        var zDist = (xMax - center.z) * 2;

        var dist = xDist > zDist ? xDist : zDist;

        /*
        ScreenshotCamera.transform.position = center;
        ScreenshotCamera.transform.position += Vector3.up * dist;
        ScreenshotCamera.transform.LookAt(center);
        */

        var cam = Camera.main.transform;
        var dir = center - cam.position;
        dir.Normalize();

        ScreenshotCamera.transform.position = center - (dir * dist);
        StartCoroutine(Screenshot(transforms, data.Id));

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SymbolPath + data.Id + ".json", json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    IEnumerator Screenshot(Transform[] transforms, string id)
    {
        GL.Clear(true, true, Color.black);

        foreach (var t in transforms)
        {
            var meshes = t.GetComponentsInChildren<MeshRenderer>();
            foreach (var m in meshes)
            {
                m.gameObject.layer = 10;
            }
        }

        yield return new WaitForSeconds(0.1F);

        byte[] bytes = RenderTextureToTexture2D(ScreenshotRenderTexture).EncodeToPNG();
        File.WriteAllBytes(SymbolPath + "image-" + id + ".png", bytes);

        yield return new WaitForSeconds(0.1F);

        foreach (var t in transforms)
        {
            var meshes = t.GetComponentsInChildren<MeshRenderer>();
            foreach (var m in meshes)
            {
                m.gameObject.layer = 9;
            }
        }

        ScreenshotRenderTexture.Release();
    }

    Texture2D RenderTextureToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(256, 256, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    // Load all Json inside Presets
    [ContextMenu("Load presets")]
    public void LoadPresets()
    {
        var info = new DirectoryInfo(SymbolPath);
        var fileInfo = info.GetFiles();

        foreach (var file in fileInfo)
        {
            var fileName = file.FullName;

            if (fileName.Contains(".meta"))
                continue;

            if (fileName.Contains(".png"))
                continue;

            var symbolData = LoadSymbol(fileName);
            Presets.Add(symbolData);

            _presetLookUp.Add(symbolData.Id, symbolData);
        }

        // Symbol Manager need to populate it
        var symbolManager = SymbolManager.Instance;
        symbolManager.PopulatePresets(Presets);
    }

    internal void ReloadSymbols()
    {
        Presets.Clear();
        _presetLookUp.Clear();

        LoadPresets();
    }


    public AppData LoadSymbol(string path)
    {
        Debug.Log(path);
        var data = File.ReadAllText(path);
        var appData = JsonUtility.FromJson<AppData>(data);

        var iconBytes = File.ReadAllBytes(SymbolPath + "image-" + appData.Id + ".png");
        var tx = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        tx.LoadImage(iconBytes);

        appData.IconTexture2D = tx;

        return appData;
    }


    public AppData GetSymbol(string id)
    {
        return _presetLookUp[id];
    }

    public void Save()
    {
        m_AppData.m_Blocks.Clear();
        var blocks = FindObjectsOfType<Block>();

        foreach (var block in blocks)
        {
            block.Save();
            m_AppData.m_Blocks.Add(block.mBlockData);
        }

        var json = JsonUtility.ToJson(m_AppData, true);
        File.WriteAllText(SaveDataPath, json);
    }

    public void Load()
    {
        var blocks = FindObjectsOfType<Block>();

        foreach (var block in blocks)
        {
            Destroy(block.gameObject);
        }

        var data = File.ReadAllText(SaveDataPath);
        var jsonData = JsonUtility.FromJson<AppData>(data);

        foreach (var block in jsonData.m_Blocks)
        {
            var instance = Instantiate(blockPrefab);
            instance.gameObject.transform.position = block.position;
            instance.gameObject.transform.localScale = block.scale;
        }
    }

    public Block CreateBlock(Block.BlockData data)
    {
        var instance = Instantiate(blockPrefab);
        instance.gameObject.transform.position = data.position;
        instance.gameObject.transform.localScale = data.scale;

        return instance;
    }

    public void New()
    {
        SceneManager.LoadScene(0);
    }
}
