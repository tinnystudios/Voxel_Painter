using UnityEngine;
using System.IO;

public class ObjExportController : MonoBehaviour 
{
    public GameObject TargetObject;

    public void Export() 
    {
        string path = Path.Combine(Application.persistentDataPath, "data");
        path = Path.Combine(path, "carmodel" + ".obj");

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        MeshFilter meshFilter = TargetObject.GetComponent<MeshFilter>();
        //ObjExporter.MeshToFile(meshFilter, path);
    }
}