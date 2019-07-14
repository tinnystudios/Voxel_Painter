using core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtrudeAction : MonoBehaviour, IAction
{
    public CreateBlockAction CreateBlockAction;

    private Vector3 _firstPos;
    private List<Block> _blocks = new List<Block>();

    private void Update()
    {
        var selectedAction = ActionManager.Instance.selectedAction;
        if (selectedAction.Result.Equals(this))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 10;

            var pos = Camera.main.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButtonDown(0))
            {
                _firstPos = pos;
            }

            if (Input.GetMouseButton(0))
            {
                // Create and destroy based on the Delta X. 
                var delta = pos - _firstPos;

                if (delta.x >= 1)
                {
                    NewBlock();

                    // Reset.
                    _firstPos = pos;
                }
                else if (delta.x <= -1)
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
        var face = SelectionManager.Instance.selectedGameObjects[0].GetComponent<Face>();
        var faceType = face.FaceType;

        var block = face.Block;

        _blocks.Remove(block);
        Destroy(block.gameObject);

        // Select previous
        if (_blocks.Count > 0)
        {
            var currentBlock = _blocks.LastOrDefault();
            var newFace = currentBlock.faces.FirstOrDefault(x => x.FaceType == faceType);
            SelectionManager.Instance.Deselect(face);
            SelectionManager.Instance.Select(newFace);
        }
    }

    public void NewBlock()
    {
        if (!SelectionManager.Instance.HasSelection)
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
