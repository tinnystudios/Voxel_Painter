using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : Singleton<ToolTipManager>
{
    private Camera mCamera;
    public RectTransform m_ToolTipTransform;
    public Canvas m_Canvas;
    public Text m_ToolTipDescriptions;

    public Vector3 m_Offset;

    private void Awake()
    {
        HideToolTip();
        mCamera = m_Canvas.worldCamera;
    }

    public void MoveToPointer()
    {
        var pos = Input.mousePosition + m_Offset;
        pos.z = m_Canvas.planeDistance;
        m_ToolTipTransform.position = mCamera.ScreenToWorldPoint(pos);
    }

    public void ShowToolTip(IToolTip m_ToolTip)
    {
        MoveToPointer();
        m_ToolTipDescriptions.text = m_ToolTip.Description;
        m_ToolTipTransform.gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        m_ToolTipTransform.gameObject.SetActive(false);
    }
}
