using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;
using static SimpleFileBrowser.FileBrowser;

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

        Debug.Log(SaveDataPath);
    }

    public void SaveSymbol(List<Block> blocks, string id = null)
    {
        if (blocks.Count == 0)
            return;

        var data = new AppData();

        data.Id = id ?? Guid.NewGuid().ToString();

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
        var yMax = TransformUtils.YMax(transforms);

        var yDist = (yMax - center.y) * 2;
        var xDist = (xMax - center.x) * 2;
        var zDist = (zMax - center.z) * 2;

        var list = new List<float>();
        list.Add(yDist);
        list.Add(xDist);
        list.Add(zDist);

        var dist = list.Max() * 2;

        /*
        ScreenshotCamera.transform.position = center;
        ScreenshotCamera.transform.position += Vector3.up * dist;
        ScreenshotCamera.transform.LookAt(center);
        */

        var cam = Camera.main.transform;
        var dir = center - cam.position;
        dir.Normalize();

        ScreenshotCamera.transform.position = center - (dir * dist);
        ScreenshotCamera.transform.forward = dir;
        StartCoroutine(Screenshot(transforms, data.Id));

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SymbolPath + data.Id + ".json", json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public string MakeSymbolPath(string id)
    {
        return SymbolPath + id + ".json";
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
        FileBrowser.SetFilters(false, new Filter("Json", ".json"));
        FileBrowser.ShowSaveDialog((path) => 
        {
            m_AppData.m_Blocks.Clear();
            var blocks = FindObjectsOfType<Block>();

            foreach (var block in blocks)
            {
                block.Save();
                m_AppData.m_Blocks.Add(block.mBlockData);
            }

            var json = JsonUtility.ToJson(m_AppData, true);
            File.WriteAllText(path, json);
        }, 
        () => { });
    }

    public void Load()
    {
        FileBrowser.SetFilters(false, new Filter("Json", ".json"));
        FileBrowser.ShowLoadDialog((path) => 
        {
            var prefabs = FindObjectsOfType<Prefab>();
            foreach (var p in prefabs)
                Destroy(p.gameObject);

            var blocks = FindObjectsOfType<Block>();
            foreach (var block in blocks)
                Destroy(block.gameObject);

            var data = File.ReadAllText(path);
            var jsonData = JsonUtility.FromJson<AppData>(data);

            foreach (var block in jsonData.m_Blocks)
            {
                var instance = Instantiate(blockPrefab);
                instance.gameObject.transform.position = block.position;
                instance.gameObject.transform.localScale = block.scale;
                instance.Load(block);

                CreatePrefabForBlock(instance);
            }

            // Go through everything prefab and see if they need updating?
            foreach (var prefab in _prefabLookup.ToList().Select(x => x.Value))
            {
                prefab.UpdateChanges(destroy: true);
            }
        }, 
        () => 
        {

        });

        return;
    }

    public Block CreateBlock(Block.BlockData data)
    {
        var instance = Instantiate(blockPrefab);
        instance.gameObject.transform.position = data.position;
        instance.gameObject.transform.localScale = data.scale;
        instance.Load(data);
        return instance;
    }

    public void New()
    {
        SceneManager.LoadScene(0);
    }

    public void CreatePrefabForBlock(Block block)
    {
        var data = block.mBlockData;

        if (!string.IsNullOrEmpty(data.PrefabData.Id))
        {
            var pData = data.PrefabData;

            if (!_prefabLookup.ContainsKey(data.PrefabData.GuidInstance))
            {
                var prefab = new GameObject("Symbol").AddComponent<Prefab>();
                var list = new List<Block>();
                prefab.Setup(pData.Id, list, pData.SelectedPosition, guid: pData.GuidInstance);
                _prefabLookup.Add(pData.GuidInstance, prefab);
            }

            var prefabInstance = _prefabLookup[pData.GuidInstance];
            prefabInstance.Add(block);
        }
    }

    private Dictionary<string, Prefab> _prefabLookup = new Dictionary<string, Prefab>();
}
