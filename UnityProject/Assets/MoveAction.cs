using core;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour, IAction, IShortKey
{
    public List<PositionContainerList> undoList;
    public List<PositionContainerList> redoList;

    public Action Action;
    public Axes Axes;

    public KeyCode Key
    {
        get
        {
            return InputManager.Instance.Config.Move;
        }
    }
    
    private void Awake()
    {
        Axes.OnMove += OnAxesMove;
        Axes.OnMoveCompleted += OnAxesFinished;

        Axes.gameObject.SetActive(false);
    }

    // Moving
    private void OnAxesMove()
    {
        var pivot = SelectionManager.Instance.pivot;
        pivot.transform.position = Axes.transform.position;
    }

    // Moved!
    private void OnAxesFinished()
    {
        var pivot = SelectionManager.Instance.pivot;

        var delta = pivot.transform.position - _pivotLastPosition;

        if (_pivotLastPosition != pivot.transform.position)
        {
            HistoryManager.Instance.AddAction(Action);
            _pivotLastPosition = pivot.transform.position;

            AddUndo(delta);
        }
    }

    private void AddUndo(Vector3 delta)
    {
        var blocks = SelectionManager.Instance.blocks;
        var posContainerList = new PositionContainerList();

        foreach (var block in blocks)
        {
            var posContainer = new PositionContainer();
            posContainer.transform = block.transform;
            posContainer.lastPosition = block.transform.position - delta;
            posContainer.newPosition = transform.position;
            posContainerList.list.Add(posContainer);
        }
        undoList.Add(posContainerList);
    }

    public void UpdateAction()
    {

    }

    public void Deselect()
    {
        Axes.gameObject.SetActive(false);
    }

 
    public void Select()
    {
        if (SelectionManager.Instance.selectedGameObjects.Count == 0)
            return;

        Axes.gameObject.SetActive(true);

        var pivot = SelectionManager.Instance.pivot;
        Axes.gameObject.transform.position = pivot.position;

        _pivotLastPosition = pivot.transform.position;
    }

    public bool Use()
    {
        return false;
    }

    public void Undo()
    {
        var element = undoList[undoList.Count - 1];

        foreach (var t in element.list)
        {
            t.transform.position = t.lastPosition;
        }

        undoList.Remove(element);
        redoList.Add(element);

        // Recalculator pivot point
        SelectionManager.Instance.CheckPivot();
    }

    public void Redo()
    {
        var element = redoList[redoList.Count - 1];

        foreach (var t in element.list)
        {
            t.transform.position = t.newPosition;
        }

        undoList.Add(element);
        redoList.Remove(element);
    }

    private Vector3 _pivotLastPosition;
}

[System.Serializable]
public class PositionContainer
{
    public Transform transform;
    public Vector3 lastPosition;
    public Vector3 newPosition;
}

[System.Serializable]
public class PositionContainerList
{
    public List<PositionContainer> list = new List<PositionContainer>();
}