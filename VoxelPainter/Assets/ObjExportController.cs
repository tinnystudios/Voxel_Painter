using UnityEngine;
using System.IO;
using static ObjExporterScript;

public class ObjExportController : MonoBehaviour 
{
    public GameObject TargetObject;

    public void Export() 
    {
        ObjExporterMain.DoExport(true, TargetObject.transform, "test", Application.dataPath + "/_Project/Playground/test.obj");
    }
}