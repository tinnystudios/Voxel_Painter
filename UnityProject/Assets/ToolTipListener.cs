using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public float m_Duration = 2;
    private float startTime = 0;

    private core.Action m_Action;
    private IToolTipDisplay mToolTipDisplay;

    void Awake()
    {
        mToolTipDisplay = GetComponent<IToolTipDisplay>();
    }

    void Update()
    {
        if (startTime != 0)
        {
            if (Time.time - startTime >= m_Duration)
            {
                ToolTipManager.Instance.ShowToolTip(mToolTipDisplay.ToolTip);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (startTime == 0)
            startTime = Time.time;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        startTime = 0;

        ToolTipManager.Instance.HideToolTip();
    }
}
