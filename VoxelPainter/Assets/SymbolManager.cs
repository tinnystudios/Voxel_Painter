using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolManager : Singleton<SymbolManager>
{
    public void Add()
    {
        var blocks = SelectionManager.Instance.blocks;
        ApplicationManager.Instance.SaveSymbol(blocks);
    }

    public void Load(Vector3 selectedBlockPosition, string id)
    {
        var data = ApplicationManager.Instance.LoadSymbol();
        var blocks = data.m_Blocks;

        var transforms = new List<Transform>();

        foreach (var blockData in blocks)
        {
            var instance = ApplicationManager.Instance.CreateBlock(blockData);
            transforms.Add(instance.transform);
        }

        var tempCenter = TransformUtils.Center(transforms.ToArray());
        var pivot = new GameObject("Symbol");

        pivot.transform.position = tempCenter;

        foreach (var t in transforms)
        {
            t.SetParent(pivot.transform);
        }

        pivot.transform.position = selectedBlockPosition;
        //pivot.transform.position += Vector3.up * 0.5F;

        // Find highest piece
        float highest = 0;

        foreach (var t in transforms)
        {
            if (t.position.y > highest)
                highest = t.position.y;
        }

        float scale = highest - pivot.transform.position.y;
        pivot.transform.position += Vector3.up * scale;
    }
}
