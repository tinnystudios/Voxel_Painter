using UnityEngine;
using UnityEngine.EventSystems;
using core;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SlotButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IToolTipDisplay, IPointerDownHandler, IPointerExitHandler {

    public SlotGroup m_SlotGroup;
    public float m_Duration = 1.0F;
    private float mStartTime = 0;

    public Image image;
    public core.Action action;
    public GameObject highlight;

    void Awake()
    {
        if (m_SlotGroup != null)
        {
            m_SlotGroup.OnSlotGroupActionSelected += OnSlotGroupActionSelected;
        }
        else
        {

        }
    }

    public void Select(Sprite spirte)
    {
        highlight.gameObject.SetActive(true);
        image.sprite = spirte;
    }

    public void Deselect()
    {
        highlight.gameObject.SetActive(false);
    }

    private void OnSlotGroupActionSelected(core.Action obj)
    {
        Init(obj);
    }

    void Update()
    {
        if (mStartTime != 0 && Time.time - mStartTime >= m_Duration)
        {
            if (m_SlotGroup != null)
            {
                m_SlotGroup.Activate();
            }
        }
    }

    public IToolTip ToolTip
    {
        get
        {
            var toolTip = action.Result.gameObject.GetComponent<IToolTip>();
            return toolTip;
        }
    }

    public void Init(core.Action action)
    {
        var actionData = action.Result.gameObject.GetComponent<ActionData>();
        this.action = action;

        if (actionData != null)
        {
            image.sprite = actionData.m_Sprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ActionManager.Instance.SelectAction(action);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(mStartTime == 0.0F)
            mStartTime = Time.time;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mStartTime = 0;
    }
}
