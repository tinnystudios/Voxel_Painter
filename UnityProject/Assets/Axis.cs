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

        if (IsDragging)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            _delta = new Vector3(h, v, 0);

            var delta = AxisType == AxisType.Up ? _delta.y : _delta.x;

            Axes.transform.position += _dir * Speed * Time.deltaTime * delta;
        }

        if (Input.GetMouseButtonUp(0))
            IsDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragging = true;
    }

    public bool IsDragging = false;
    private Vector3 _dir;
    private Vector3 _delta;
}

public enum AxisType
{
    Forward,
    Up,
    Right
}