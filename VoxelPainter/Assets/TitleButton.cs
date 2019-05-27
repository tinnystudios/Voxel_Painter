using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerClickHandler
{
    public bool IsOpened = false;
    public GameObject Group;

    private void Awake()
    {
        ApplyGroupVisibility();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsOpened = !IsOpened;
        ApplyGroupVisibility();
    }

    public void ApplyGroupVisibility()
    {
        Group.SetActive(IsOpened);
    }
}
