using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymbolManager : Singleton<SymbolManager>
{
    public void Add()
    {
        var blocks = SelectionManager.Instance.blocks;
        ApplicationManager.Instance.SaveSymbol(blocks);
    }

    public void Load(Block selectedBlock, string id)
    {
        var selectedBlockPosition = selectedBlock.transform.position;
        selectedBlockPosition.y += selectedBlock.transform.localScale.y;

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

        Transform lowest = transforms[0];

        foreach(var t in transforms)
        {
            if (t.position.y < lowest.position.y)
            {
                lowest = t;
            }
        }

        var lowestFace = lowest.GetComponentsInChildren<Face>()
            .OrderBy(x => x.transform.position.y)
            .Take(1)
            .SingleOrDefault();

        var selectedFace = selectedBlock.GetComponentsInChildren<Face>()
            .OrderByDescending(x => x.transform.position.y)
            .Take(1)
            .SingleOrDefault();

        pivot.transform.position = lowestFace.transform.position;

        // Add to symbol position
        foreach (var t in transforms)
        {
            t.SetParent(pivot.transform);
        }

        pivot.transform.position = selectedFace.transform.position;
    }
}