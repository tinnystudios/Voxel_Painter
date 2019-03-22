using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerClickHandler
{
    private bool _isOpened = false;
    public GameObject Group;

    private void Awake()
    {
        ApplyGroupVisibility();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isOpened = !_isOpened;
        ApplyGroupVisibility();
    }

    public void ApplyGroupVisibility()
    {
        Group.SetActive(_isOpened);
    }
}
