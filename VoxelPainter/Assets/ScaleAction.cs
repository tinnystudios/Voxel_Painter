using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//There are 2 types of scale, locally or world space
public class ScaleAction : MonoBehaviour, IAction, IShortKey
{
    public List<ScaleContainerList> undoList;
    public List<ScaleContainerList> redoList;

    public GameObject ScaleGroup;

    public KeyCode Key
    {
        get
        {
            return InputManager.Instance.Config.Scale;
        }
    }


    public void UpdateAction()
    {

    }

    public void Undo()
    {
        var element = undoList[undoList.Count - 1];

        foreach (var t in element.list)
        {
            t.transform.localScale = t.lastScale;
        }

        undoList.Remove(element);
        redoList.Add(element);
    }

    public void Redo()
    {
        var element = redoList[redoList.Count - 1];

        foreach (var t in element.list)
        {
            t.transform.localScale = t.newScale;
        }

        undoList.Add(element);
        redoList.Remove(element);
    }

    public void Deselect()
    {
        ScaleGroup.SetActive(false);
    }

    public void Select()
    {
        ScaleGroup.SetActive(true);
    }

    public bool Use()
    {
        ScaleLocally(CreationManager.Instance.mSize);
        return true;
    }

    public void ScaleLocally(float size) {
        var blocks = SelectionManager.Instance.blocks;
        var scaleContainerList = new ScaleContainerList();

        bool hasChanged = false;

        foreach (var block in blocks) {
            var scaleContainer = new ScaleContainer();
            scaleContainer.transform = block.transform;
            scaleContainer.lastScale = block.transform.localScale;
            scaleContainer.newScale = Vector3.one * size;
            scaleContainerList.list.Add(scaleContainer);

            if (block.transform.localScale != Vector3.one * size)
            {
                hasChanged = true;
                block.transform.localScale = Vector3.one * size;
            }
        }

        if(hasChanged)
            undoList.Add(scaleContainerList);
    }

    public void ScaleWorldly(float size) {
        var blocks = SelectionManager.Instance.blocks;
        Transform pivot = SelectionManager.Instance.pivot;

        //Scale to size
        pivot.transform.localScale = Vector3.one * size;

        //Scale to
    }

    [System.Serializable]
    public class ScaleContainer {
        public Transform transform;
        public Vector3 lastScale;
        public Vector3 newScale;
    }

    [System.Serializable]
    public class ScaleContainerList {
        public List<ScaleContainer> list = new List<ScaleContainer>();
    }
}

/*
        Transform pivot = SelectionManager.Instance.pivot;
        Block[] blocks = pivot.GetComponentsInChildren<Block>();

        foreach (Block block in blocks)
            block.transform.SetParent(null);

        //Returning to whatever size
        pivot.transform.localScale = Vector3.one * undoList[undoList.Count-1];

        foreach (Block block in blocks)
            block.transform.SetParent(pivot.transform);

        //Return to full size
        pivot.transform.localScale = Vector3.one;

        redoList.Add(undoList[undoList.Count - 1]);
        undoList.RemoveAt(undoList.Count - 1);
*/