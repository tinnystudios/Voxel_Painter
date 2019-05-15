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

    private List<Transform> GenerateBlocks(string id)
    {
        var data = ApplicationManager.Instance.GetSymbol(id);
        var blocks = data.m_Blocks;
        var transforms = new List<Transform>();

        foreach (var blockData in blocks)
        {
            var instance = ApplicationManager.Instance.CreateBlock(blockData);
            transforms.Add(instance.transform);
        }

        return transforms;
    }

    private Transform Lowest(List<Transform> transforms)
    {
        Transform lowest = transforms[0];

        foreach (var t in transforms)
        {
            if (t.position.y < lowest.position.y)
                lowest = t;
        }

        return lowest;
    }

    public void Load(Block selectedBlock, string id)
    {
        var transforms = GenerateBlocks(id);
        var lowestBlock = Lowest(transforms);
        var lowestFace = LowestFace(lowestBlock);
        var selectedFace = HighestFace(selectedBlock.transform);

        var pivot = SetupPivot(lowestFace, transforms);
        pivot.position = selectedFace.transform.position;
    }

    private Face LowestFace(Transform block)
    {
        var lowestFace = block.GetComponentsInChildren<Face>()
            .OrderBy(x => x.transform.position.y)
            .Take(1)
            .SingleOrDefault();

        return lowestFace;
    }

    private Face HighestFace(Transform block)
    {
        var lowestFace = block.GetComponentsInChildren<Face>()
            .OrderByDescending(x => x.transform.position.y)
            .Take(1)
            .SingleOrDefault();

        return lowestFace;
    }

    private Transform SetupPivot(Face lowestFace, List<Transform> transforms)
    {
        var pivot = new GameObject("Symbol");
        pivot.transform.position = lowestFace.transform.position;

        foreach (var t in transforms)
            t.SetParent(pivot.transform);

        return pivot.transform;
    }
}