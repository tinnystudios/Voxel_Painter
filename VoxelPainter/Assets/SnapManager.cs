using System;
using UnityEngine;
using App;

public class SnapManager : Singleton<SnapManager>
{
    public SnapSettings Settings = new SnapSettings();
    public Transform VertexCursor;
    public MoveAction MoveAction;
    public LayerMask Mask;

    private void Awake()
    {
        InputManager.Instance.OnInputVertexSnap += OnInputVertexSnap;
    }

    public RaycastHit GetMouseHit()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var hit, Mask);
        return hit;
    }

    private void OnInputVertexSnap()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit,1000, Mask))
        {
            Debug.Log(hit.transform.name);
            var mesh = hit.transform.GetComponent<MeshFilter>().mesh;
            VertexCursor.position = hit.transform.NearestVertexTo(hit.point, mesh);

            if (!Input.GetMouseButton(0))
            {
                SelectionManager.Instance.SetPivotPosition(VertexCursor.position);
                MoveAction.SetAxisToPivot();
            }

            var pivot = SelectionManager.Instance.pivot;

            if (pivot.position != VertexCursor.position)
            {
                pivot.position = VertexCursor.position;
                MoveAction.SetAxisToPivot();
            }
        }
    }

    public void SetSnapSize(string sizeString)
    {
        SetSnapSize(float.Parse(sizeString));
    }

    public void SetSnapSize(float size)
    {
        Settings.GridSize = size;
    }

    public void SnapType(ESnapType type)
    {
        Settings.Type = type;
    }
}

public class SnapSettings
{
    public ESnapType Type = ESnapType.Grid;
    public float GridSize = 1.0F;
}

public enum ESnapType
{
    None,
    Grid,
    Point
}
