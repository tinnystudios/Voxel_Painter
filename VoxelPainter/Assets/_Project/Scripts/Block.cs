using System;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockData mBlockData;

    public Face[] faces;

    public HashSet<Face> faceLookUp = new HashSet<Face>();

    private void Awake()
    {
        foreach (Face face in faces)
            faceLookUp.Add(face);
    }
    public void SetColor(Color c) {
        foreach (Face face in faces)
        {
            face.SetColor(c);
        }
    }

    public void Save()
    {
        mBlockData = new BlockData();

        var facesData = new FaceData[faces.Length];
        for (int i = 0; i < faces.Length; i++)
        {
            var face = faces[i];
            var faceData = new FaceData();
            faceData.Color = face.color;
            faceData.FaceType = face.FaceType;

            facesData[i] = faceData;
        }

        mBlockData.FacesData = facesData;
        mBlockData.position = transform.position;
        mBlockData.rotation = transform.rotation;
        mBlockData.scale = transform.localScale;
    }

    [System.Serializable]
    public class BlockData
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;

        public FaceData[] FacesData;
    }

    public void Load(BlockData data)
    {
        Dictionary<EFaceType, FaceData> faceMap = new Dictionary<EFaceType, FaceData>();

        foreach (var f in data.FacesData)
        {
            faceMap.Add(f.FaceType, f);
        }

        foreach (var f in faces)
        {
            var fData = faceMap[f.FaceType];
            f.SetColor(fData.Color);
        }
    }
}
