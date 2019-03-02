using core;
using UnityEngine;

public class MoveAction : MonoBehaviour, IAction
{
    public Action Action;
    public Axes Axes;

    private void Awake()
    {
        Axes.OnMove += OnAxesMove;
        Axes.OnMoveCompleted += OnAxesFinished;

        Axes.gameObject.SetActive(false);
    }

    private void OnAxesMove()
    {
        var pivot = SelectionManager.Instance.pivot;
        pivot.transform.position = Axes.transform.position;
    }

    private void OnAxesFinished()
    {
        var pivot = SelectionManager.Instance.pivot;

        // Has moved
        if (_pivotLastPosition != pivot.transform.position)
        {
            HistoryManager.Instance.AddAction(Action);
        }
    }

    public void UpdateAction()
    {

    }

    public void Deselect()
    {
        Axes.gameObject.SetActive(false);
    }

    public void Redo()
    {

    }

    public void Select()
    {
        Axes.gameObject.SetActive(true);

        var pivot = SelectionManager.Instance.pivot;
        Axes.gameObject.transform.position = pivot.position;

        _pivotLastPosition = pivot.transform.position;
    }

    public void Undo()
    {

    }

    public bool Use()
    {
        return false;
    }

    private Vector3 _pivotLastPosition;
}
