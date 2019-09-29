using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPanel : MonoBehaviour, IPointerClickHandler
{
    public static Action<MainPanel> OnPanelSelected;

    public SubPanel m_SubPanel;
    private bool mOpened = false;

    void Awake()
    {
        m_SubPanel.gameObject.SetActive(mOpened);
        OnPanelSelected += OnPanelChanged;
    }

    void OnDestroy()
    {
        OnPanelSelected -= OnPanelChanged;
    }

    private void OnPanelChanged(MainPanel obj)
    {
        if (obj != this)
        {
            mOpened = false;
            m_SubPanel.gameObject.SetActive(mOpened);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mOpened = !mOpened;
        m_SubPanel.gameObject.SetActive(mOpened);

        if (OnPanelSelected != null)
            OnPanelSelected.Invoke(this);
    }
}
