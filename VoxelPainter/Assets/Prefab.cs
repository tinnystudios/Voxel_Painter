using System;
using System.Collections.Generic;
using UnityEngine;

public class Prefab : MonoBehaviour
{
    public string Id { get; private set; }
    public string InstanceGuid { get; set; }

    public List<Block> Children { get; set; }
    public Vector3 SelectedPosition { get; set; }

    public void Setup(string id, List<Block> children, Vector3 selectedPosition, string guid = "")
    {
        Id = id;
        Children = children;
        SelectedPosition = selectedPosition;

        InstanceGuid = string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid;

        var data = new PrefabData()
        {
            Id = id,
            GuidInstance = InstanceGuid,
            SelectedPosition = SelectedPosition,
        };

        foreach (var block in children)
        {
            block.mBlockData.PrefabData = data;
        }
    }

    public void Add(Block block)
    {
        if (block == null)
            return;

        if (transform == null)
            return;

        Children.Add(block);
        block.transform.SetParent(transform);
    }

    public void UpdateChanges(bool destroy = false)
    {
        try
        {
            foreach (var child in Children)
            {
                child?.gameObject?.SetActive(false);
                if (child != null && destroy)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        catch { }
        Children.Clear();

        SymbolManager.Instance.Load(this, Id);
    }

    // save load
}
