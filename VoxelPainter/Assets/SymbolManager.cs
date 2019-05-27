using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymbolManager : Singleton<SymbolManager>
{
    public Transform SymbolContainer;
    public SymbolButton SymbolPrefab;

    private List<SymbolButton> _symbolButtons = new List<SymbolButton>();

    private void Awake()
    {
        // Symbol list load all from X path
    }

    public void Add()
    {

        var blocks = SelectionManager.Instance.blocks;
        ApplicationManager.Instance.SaveSymbol(blocks);

        // Clear all
        foreach (var symbol in _symbolButtons)
        {
            Destroy(symbol.gameObject);
        }

        _symbolButtons.Clear();

        // Reload
        ApplicationManager.Instance.ReloadSymbols();
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

    internal void PopulatePresets(List<AppData> presets)
    {
        foreach (var preset in presets)
        {
            var symbol = Instantiate(SymbolPrefab, SymbolContainer);
            symbol.Model = new SymbolModel { Id = preset.Id };

            _symbolButtons.Add(symbol);
        }
    }
}