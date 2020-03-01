using UnityEngine;
using System.IO;
using static ObjExporterScript;

public class ObjExportController : MonoBehaviour 
{
    public GameObject TargetObject;

    [ContextMenu("Export")]
    public void Export() 
    {
        ObjExporterMain.DoExport(false, TargetObject.transform, "test", Application.dataPath + "/_Project/Playground/test.obj");
    }
}