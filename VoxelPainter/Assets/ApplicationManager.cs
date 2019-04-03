using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AppData
{
    public List<Block.BlockData> m_Blocks = new List<Block.BlockData>();
    public Vector3 Center;
}

public class ApplicationManager : Singleton<ApplicationManager>
{
    public AppData m_AppData;

    private string SaveDataPath = "/SaveData/";
    private string SymbolPath = "/Presets/";

    private string SaveDataFileName = "SaveData.json";
    private string SymbolFileName = "test.json";

    public Block blockPrefab;

    private void Awake()
    {
        var rootPath = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
        SaveDataPath = rootPath + SaveDataPath;
        SymbolPath = rootPath + SymbolPath;

        Directory.CreateDirectory(SaveDataPath);
        Directory.CreateDirectory(SymbolPath);

        SaveDataPath += SaveDataFileName;
        SymbolPath += SymbolFileName;
    }

    public void SaveSymbol(List<Block> blocks)
    {
        var data = new AppData();

        foreach (var block in blocks)
        {
            block.Save();
            data.m_Blocks.Add(block.mBlockData);
        }

        var transforms = blocks.Select(x => x.transform).ToArray();
        var center = TransformUtils.Center(transforms);

        data.Center = center;

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SymbolPath, json);
    }

    public AppData LoadSymbol()
    {
        var data = File.ReadAllText(SymbolPath);
        var appData = JsonUtility.FromJson<AppData>(data);

        return appData;
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
