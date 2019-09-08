using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SymbolManager : Singleton<SymbolManager>
{
    public Transform SymbolContainer;
    public SymbolButton SymbolPrefab;

    public PrefabContextMenu PrefabContextMenu;

    private List<SymbolButton> _symbolButtons = new List<SymbolButton>();

    public SymbolButton SelectedSymbolButton { get; private set; }

    public void Save(string id = null)
    {
        var blocks = SelectionManager.Instance.blocks;
        ApplicationManager.Instance.SaveSymbol(blocks, id);
        ReloadSymbolButtons();

        // TODO
        if (id != null)
        {
            // Update all instances.
        }
    }

    public void Delete(string id)
    {
        File.Delete(ApplicationManager.Instance.MakeSymbolPath(id));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void ReloadSymbolButtons()
    {
        foreach (var symbol in _symbolButtons)
        {
            Destroy(symbol.gameObject);
        }

        _symbolButtons.Clear();
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.3F);
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

        var pivot = SetupPivot(lowestFace, transforms, id);
        pivot.position = selectedFace.transform.position;
    }

    public void SelectSymbolButton(SymbolButton symbolButton)
    {
        SelectedSymbolButton = symbolButton;
        PrefabContextMenu.SetContext(symbolButton.Model.Id);
        // Show context menu? Click else where, hide context menu?
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

    private Transform SetupPivot(Face lowestFace, List<Transform> transforms, string id)
    {
        var pivot = new GameObject("Symbol");
        pivot.transform.position = lowestFace.transform.position;

        var prefabInstance = pivot.AddComponent<Prefab>();
        prefabInstance.SetId(id);

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
            symbol.SetIcon(preset.IconTexture2D);
            _symbolButtons.Add(symbol);
        }
    }
}
