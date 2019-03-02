using UnityEngine;

public class MoveAction : MonoBehaviour, IAction
{
    public Axes Axes;

    private void Update()
    {
        var pivot = SelectionManager.Instance.pivot;
        //Axes.gameObject.transform.position = pivot.position;
    }

    public void Deselect()
    {

    }

    public void Redo()
    {

    }

    public void Select()
    {

    }

    public void Undo()
    {

    }

    public bool Use()
    {
        return true;
    }
}
