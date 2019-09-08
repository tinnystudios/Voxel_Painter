using System;
using System.Collections.Generic;
using UnityEngine;

public class Prefab : MonoBehaviour
{
    public string Id { get; private set; }
    public List<Block> Children { get; set; }
    public Vector3 SelectedPosition { get; set; }

    public void Setup(string id, List<Block> children, Vector3 selectedPosition)
    {
        Id = id;
        Children = children;
        SelectedPosition = selectedPosition;
    }

    public void UpdateChanges()
    {
        foreach (var child in Children)
            child.gameObject.SetActive(false);

        Children.Clear();

        SymbolManager.Instance.Load(this, Id);
    }
}
