using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFBXExporter;
using System.IO;

public class FbxExportController : MonoBehaviour
{
    public GameObject TargetObject;

    public void Export()
    {
        string path = Path.Combine(Application.persistentDataPath, "data");
        path = Path.Combine(path, "carmodel" + ".fbx");

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        FBXExporter.ExportGameObjToFBX(TargetObject, path, true, true);
    }
}
