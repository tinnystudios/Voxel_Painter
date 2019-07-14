using core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtrudeAction : MonoBehaviour, IAction
{
    public CreateBlockAction CreateBlockAction;

    private Vector3 _firstPos;
    private List<Block> _blocks = new List<Block>();

    public bool Cannot => SelectionManager.Instance.HasMoreThan1 || !SelectionManager.Instance.HasSelection;

    private void Update()
    {
        if (Cannot)
            return;

        var selectedAction = ActionManager.Instance.selectedAction;
        if (selectedAction.Result.Equals(this))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 10;

            var pos = Camera.main.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                _blocks.Clear();

                var face = SelectionManager.Instance.selectedGameObjects[0].GetComponent<Face>();
                var block = face.Block;

                _blocks.Add(block);

                _firstPos = pos;
            }

            if (Input.GetMouseButton(0))
            {
                // Create and destroy based on the Delta X. 
                var delta = pos - _firstPos;
                var face = SelectionManager.Instance.selectedGameObjects[0].GetComponent<Face>();
                var value = delta.x;

                if (value >= 1)
                {
                    NewBlock();

                    // Reset.
                    _firstPos = pos;
                }
                else if (value <= -1)
                {
                    // Remove block in opposite direction.
                    RemoveBlock();

                    _firstPos = pos;
                }
            }
        }
    }

    private void RemoveBlock()
    {
        if (_blocks.Count == 1)
            return;

        if (Cannot)
            return;

        var face = SelectionManager.Instance.selectedGameObjects[0].GetComponent<Face>();
        var faceType = face.FaceType;

        var block = face.Block;

        SelectionManager.Instance.Deselect(face);
        _blocks.Remove(block);
        Destroy(block.gameObject);

        // Select previous
        if (_blocks.Count > 0)
        {
            var currentBlock = _blocks.LastOrDefault();
            var newFace = currentBlock.faces.FirstOrDefault(x => x.FaceType == faceType);

            if (newFace == null)
                Debug.LogError("New face is null");

            SelectionManager.Instance.Select(newFace);
        }
    }

    public void NewBlock()
    {
        if (Cannot)
            return;

        // Add a new block when you've dragged it out enough
        var face = SelectionManager.Instance.selectedGameObjects[0].GetComponent<Face>();
        var faceType = face.FaceType;

        var block = CreateBlockAction.CreateBlock();
        var parent = face.transform.parent;

        block.transform.position = parent.position;
        var delta = CreateBlockAction.Delta(face.transform);

        block.transform.position += delta;

        var newFace = block.faces.FirstOrDefault(x => x.FaceType == faceType);
        SelectionManager.Instance.Deselect(face);
        SelectionManager.Instance.Select(newFace);

        _blocks.Add(block);
    }

    public void Deselect()
    {

    }

    public void Select()
    {

    }

    public void Redo()
    {

    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateAction()
    {
        throw new System.NotImplementedException();
    }

    public bool Use()
    {
        return false;
    }
}
