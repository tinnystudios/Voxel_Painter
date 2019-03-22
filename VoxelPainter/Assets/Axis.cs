using UnityEngine;
using UnityEngine.EventSystems;

public class Axis : MonoBehaviour, IPointerDownHandler
{
    public float Speed = 100;

    public Axes Axes;
    public CursorObject Cursor;

    public AxisType AxisType;

    public void ResolveDirection()
    {
        switch (AxisType)
        {
            case AxisType.Forward:
                _dir = Vector3.forward;
                break;

            case AxisType.Up:
                _dir = Vector3.up;
                break;

            case AxisType.Right:
                _dir = Vector3.right;
                break;
        }
    }

    private void Update()
    {
        // #TODO Resolve Direction
        var mousePos = Input.mousePosition;
        mousePos.z = transform.position.z;
        
        var pos = Camera.main.ScreenToWorldPoint(mousePos);
        var dirToCusor = Axes.transform.position - pos;
        dirToCusor.Normalize();

        if (IsDragging)
        {
            var secondPos = Cursor.transform.position;
            var disp = secondPos - _firstPos;

            switch (AxisType)
            {
                case AxisType.Forward:
                    disp.y = 0;
                    disp.x = 0;
                    break;
                case AxisType.Right:
                    disp.y = 0;
                    disp.z = 0;
                    break;
                case AxisType.Up:
                    disp.z = 0;
                    disp.x = 0;
                    break;
            }

            Axes.transform.position = _axesFirstPos + disp;

            Axes.OnMove();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(IsDragging == true)
                Axes.OnMoveCompleted();

            IsDragging = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _firstPos = Cursor.transform.position;
        _axesFirstPos = Axes.transform.position;

        IsDragging = true;
    }

    public bool IsDragging = false;
    private Vector3 _dir;
    private Vector3 _delta;

    private Vector3 _firstPos;
    private Vector3 _axesFirstPos;
}

public enum AxisType
{
    Forward,
    Up,
    Right
}