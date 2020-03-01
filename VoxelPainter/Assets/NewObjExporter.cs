using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ObjExporterScript
{
    private static int StartIndex = 0;

    public static void Start()
    {
        StartIndex = 0;
    }

    public static void End()
    {
        StartIndex = 0;
    }


    public static string MeshToString(MeshFilter mf, Transform t, string fileName, string path)
    {
        Vector3 s = t.localScale;
        Vector3 p = t.localPosition;
        Quaternion r = t.localRotation;

        int numVertices = 0;
        Mesh m = mf.sharedMesh;
        if (!m)
        {
            return "####Error####";
        }

        if (t.name == "Front")
        {
            t.forward = -t.forward;
        }

        StringBuilder sb = new StringBuilder();

        //Color name
        string mtlFileName = fileName;
        sb.Append("mtllib " + mtlFileName + ".mtl\n");

        var mat = t.GetComponent<MeshRenderer>().material;
        var color = mat.color;
        var matName = $"{color.r}{color.g}{color.b}";

        // Create a set of materials

        // A better way is to just rename it to the color.
        if (!ObjExporterMain.MaterialLookUp.Contains(matName))
        {
            // Creating MTL
            ObjExporterMain.MtlContent +=
            $@"

            newmtl {matName}
            illum 4
            Kd {color.r} {color.g} {color.b}
            Ka 0.00 0.00 0.00
            Tf 1.00 1.00 1.00
            Ni 1.00
            ";

            var directoryPath = Path.GetDirectoryName(path);
            File.WriteAllText(directoryPath + "/" + mtlFileName + ".mtl", ObjExporterMain.MtlContent);

            ObjExporterMain.MaterialLookUp.Add(matName);
        }

        foreach (Vector3 vv in m.vertices)
        {
            Vector3 v = t.TransformPoint(vv);
            numVertices++;
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 nn in m.normals)
        {
            Vector3 v = r * nn;
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }


        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }


        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            sb.Append("usemtl ").Append(matName).Append("\n");
            sb.Append ("usemap ").Append (matName).Append ("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1 + StartIndex, triangles[i + 1] + 1 + StartIndex, triangles[i + 2] + 1 + StartIndex));
            }
        }

        StartIndex += numVertices;

        if (t.name == "Front")
        {
            t.forward = -t.forward;
        }

        return sb.ToString();
    }


    public class ObjExporterMain : ScriptableObject
    {
        public static string MtlContent;
        public static HashSet<string> MaterialLookUp = new HashSet<string>();

        public static void DoExport(bool makeSubmeshes, Transform t, string name, string path)
        {
            MtlContent = "";
            MaterialLookUp.Clear();

            ObjExporterScript.Start();

            StringBuilder meshString = new StringBuilder();

            meshString.Append("#" + name + ".obj"
            + "\n#" + System.DateTime.Now.ToLongDateString()
            + "\n#" + System.DateTime.Now.ToLongTimeString()
            + "\n#-------"
            + "\n\n");

            Vector3 originalPosition = t.position;
            t.position = Vector3.zero;

            if (!makeSubmeshes)
            {
                meshString.Append("g ").Append(t.name).Append("\n");
            }
            meshString.Append(processTransform(t, makeSubmeshes, name, path));

            WriteToFile(meshString.ToString(), path);

            t.position = originalPosition;

            ObjExporterScript.End();
            Debug.Log("Exported Mesh: " + path);
        }
        
        static string processTransform(Transform t, bool makeSubmeshes, string name, string path)
        {
            StringBuilder meshString = new StringBuilder();

            meshString.Append("#" + t.name
            + "\n#-------"
            + "\n");

            if (makeSubmeshes)
            {
                meshString.Append("g ").Append(t.name).Append("\n");
            }

            MeshFilter mf = t.GetComponent<MeshFilter>();
            Renderer renderer = t.GetComponent<Renderer>();

            if (mf && renderer)
            {
                if(mf.gameObject.layer == 9)
                    meshString.Append(ObjExporterScript.MeshToString(mf, t, name, path));
            }

            for (int i = 0; i < t.childCount; i++)
            {
                meshString.Append(processTransform(t.GetChild(i), makeSubmeshes, name, path));
            }

            return meshString.ToString();
        }

        static void WriteToFile(string s, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(s);
            }
        }

    }
}