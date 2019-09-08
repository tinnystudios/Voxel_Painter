using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SymbolModel
{
    public string Id { get; set; }
}

public class SymbolButton : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IDropHandler, IPointerDownHandler, IPointerClickHandler
{
    public static Action<SymbolButton> OnMove;
    public static Action<SymbolButton> OnClick;

    public SymbolModel Model { get; set; }
    public RawImage Icon;
    // Layer actually spawn the symbol obj
    public Block SymbolObj;

    public LayerMask BlockLayer;

    private Vector3 _posBeforeDrag;
    private Face _lastFace;
    private Color _lastFaceColor;

    public float Distance => Vector3.Distance(transform.position, _posBeforeDrag);

    public void SetIcon(Texture2D texture)
    {
        Icon.texture = texture;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _posBeforeDrag = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10000, BlockLayer))
        {
            var face = hit.transform.GetComponent<Face>();

            if (_lastFace != null && face != _lastFace)
            {
                // face has changed
                _lastFace.SetColor(_lastFaceColor);
                _lastFace = null;
            }

            if (_lastFace == null)
            {
                _lastFaceColor = face.color;
                _lastFace = face;
                _lastFace.SetColor(Color.blue);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        transform.position = _posBeforeDrag;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _posBeforeDrag;

        if (_lastFace == null)
            return;

        // Spawn at last face
        var block = _lastFace.GetComponentInParent<Block>();

        var selectedBlockPosition = block.transform.position;

        SymbolManager.Instance.Load(block, Model.Id);

        _lastFace.SetColor(_lastFaceColor);
        _lastFace = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Distance when clicked {Distance}");

        if(Distance < 0.1F)
            OnClick?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _posBeforeDrag = transform.position;
    }
}
