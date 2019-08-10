using core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtrudeAction : MonoBehaviour, IAction
{
    public CreateBlockAction CreateBlockAction;
    public SelectAction SelectAction;

    private Vector3 _firstPos;
    private List<Block> _blocks = new List<Block>();
    private Action _action;

    public HistoryTracker<List<Block>> history = new HistoryTracker<List<Block>>();

    public bool Cannot => SelectionManager.Instance.HasMoreThan1 || !SelectionManager.Instance.HasSelection;

    private void Awake()
    {
        _action = new Action(GetComponent<IAction>());
    }

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

        if (_blocks.Count <= 1)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            var createdBlocks = _blocks.GetRange(1, _blocks.Count-1);
            var container = history.NewInstance<List<Block>>();
            container.mInitial = createdBlocks;
            history.undoList.Add(container);

            HistoryManager.Instance.AddAction(_action);

            var face = SelectionManager.Instance.selectedGameObjects[0].GetComponent<Face>();
            SelectAction.SelectFace(face);
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
        block.gameObject.SetActive(false);

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

        SelectBlockFace(block);

        _blocks.Add(block);
    }

    public void SelectBlockFace(Block block)
    {
        var face = SelectionManager.Instance.selectedGameObjects[0].GetComponent<Face>();
        var faceType = face.FaceType;

        var newFace = block.faces.FirstOrDefault(x => x.FaceType == faceType);

        // The first deselect need to be an action.
        SelectionManager.Instance.Deselect(face);
        SelectionManager.Instance.Select(newFace);
    }

    public void Deselect()
    {

    }

    public void Select()
    {

    }

    public void Redo()
    {
        var element = history.Redo();

        foreach (var block in element.mInitial)
        {
            block.gameObject.SetActive(true);
        }

        SelectionManager.Instance.DeselectAll();
    }

    public void Undo()
    {
        var element = history.Undo();

        foreach (var block in element.mInitial)
        {
            block.gameObject.SetActive(false);
        }
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
