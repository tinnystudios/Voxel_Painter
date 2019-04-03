using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SymbolButton : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IDropHandler
{
    // Layer actually spawn the symbol obj
    public Block SymbolObj;

    public LayerMask BlockLayer;

    private Vector3 _posBeforeDrag;
    private Face _lastFace;
    private Color _lastFaceColor;

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
        Debug.Log("Drop");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _posBeforeDrag;

        // Spawn at last face
        var block = _lastFace.GetComponentInParent<Block>();

        var selectedBlockPosition = block.transform.position;
        selectedBlockPosition.y += block.transform.localScale.y;

        string id = "test";
        SymbolManager.Instance.Load(selectedBlockPosition, id);

        _lastFace.SetColor(_lastFaceColor);
        _lastFace = null;

        Debug.Log("End");
    }
}
