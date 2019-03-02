using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AppData
{
    public List<Block.BlockData> m_Blocks = new List<Block.BlockData>();
}

public class ApplicationManager : MonoBehaviour
{
    public AppData m_AppData;

    private string path = "D:/Tin/Temp/SaveData.json";

    public Block blockPrefab;

    public void Save()
    {
        m_AppData.m_Blocks.Clear();
        //Create json
        //Look for all cube positions in the scene and save them
        var blocks = FindObjectsOfType<Block>();

        foreach (var block in blocks)
        {
            block.Save();
            m_AppData.m_Blocks.Add(block.mBlockData);
        }

        var json = JsonUtility.ToJson(m_AppData, true);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        var blocks = FindObjectsOfType<Block>();

        foreach (var block in blocks)
        {
            Destroy(block.gameObject);
        }

        var data = File.ReadAllText(path);
        var jsonData = JsonUtility.FromJson<AppData>(data);

        foreach (var block in jsonData.m_Blocks)
        {
            var instance = Instantiate(blockPrefab);
            instance.gameObject.transform.position = block.position;
            instance.gameObject.transform.localScale = block.scale;
        }

    }

    public void New()
    {
        SceneManager.LoadScene(0);
    }
}
