using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlockAction : MonoBehaviour, IAction
{
    public GameObject block;
    public float size = 1.0F;

    public List<GameObject> undoList = new List<GameObject>();
    public List<GameObject> redoList = new List<GameObject>();

    public void Undo()
    {
        GameObject go = undoList[undoList.Count - 1];
        go.SetActive(false);
        undoList.Remove(go);
        redoList.Add(go);
    }

    public void Redo()
    {
        GameObject go = redoList[redoList.Count - 1];
        go.SetActive(true);
        undoList.Add(go);
        redoList.Remove(go);
    }

    public void Deselect()
    {

    }

    public void Reset()
    {

    }

    public void Select()
    {

    }

    public void UpdateAction() { }

    public Vector3 Delta(Transform face)
    {
        Vector3 scale = Vector3.one * size;
        Vector3 dir = face.forward;

        float hitSize = face.parent.localScale.x;
        size = hitSize;

        float gap = hitSize - size;

        Vector3 blockPosition = face.parent.position;
        return dir * (size + gap / 2);
    }

    public bool Use()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Use Delta() methhod above
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 scale = Vector3.one * size;
            Vector3 dir = hit.transform.forward;

            float hitSize = hit.transform.parent.localScale.x;
            float gap = hitSize - size;

            Vector3 blockPosition = hit.transform.parent.position;
            blockPosition += dir * (size + gap / 2);

            GameObject go = Instantiate(block);

            go.GetComponent<Block>().SetColor(ColorManager.Instance.primaryColor);

            go.transform.localScale = scale;
            go.transform.position = blockPosition;
            go.transform.rotation = hit.transform.parent.rotation;

            undoList.Add(go);
            redoList.Clear();
            return true;
        }

        return false;

    }

    public Block Duplicate(Block block)
    {
        return Instantiate(block);
    }

    public Block CreateBlock()
    {
        Vector3 scale = Vector3.one * size;

        GameObject go = Instantiate(block);
        var instance = go.GetComponent<Block>();
        instance.SetColor(ColorManager.Instance.primaryColor);
        instance.transform.localScale = scale;

        return instance;
    }
}
