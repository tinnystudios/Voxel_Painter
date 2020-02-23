using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour, ISelectable
{
    public MaterialBlockField materialBlock;

    public EFaceType FaceType;
    public Color color = Color.white;

    private bool isSelected = false;

    public Block Block => GetComponentInParent<Block>();

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
    }

    public Material Mat;

    void Awake() {
        materialBlock.Init(gameObject);
        Mat = GetComponentInChildren<MeshRenderer>().material;
    }

    public void Deselect()
    {
        materialBlock.SetColor("_Color", color);
        isSelected = false;
    }

    public void HoverExit() {
        materialBlock.SetColor("_Color", color);
    }

    public void Select()
    {
        materialBlock.SetColor("_Color",Color.green);
        isSelected = true;
    }

    public void Hover()
    {
        materialBlock.SetColor("_Color", Color.yellow);
    }

    public void SetColor(Color c) {
        color = c;
        materialBlock.SetColor("_Color", color);

        Mat.color = color;
    }
}

[Serializable]
public class FaceData
{
    public Color Color;
    public EFaceType FaceType;
}

public enum EFaceType
{
    Back,
    Front,
    Top,
    Bottom,
    Left,
    Right
}