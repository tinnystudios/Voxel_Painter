using System;
using System.Collections;
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
    }
}
