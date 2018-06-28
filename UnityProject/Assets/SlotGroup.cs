using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using Action = core.Action;

public class SlotGroup : MonoBehaviour {

    public SlotButton m_Template;
    public List<Action> m_Actions;
    public Action m_SelectedAction;
    public System.Action<Action> OnSlotGroupActionSelected;

    private void Awake()
    {
        m_Template.gameObject.SetActive(false);

        foreach (var action in m_Actions)
        {
            var temp = Instantiate(m_Template,transform);
            temp.Init(action);
            temp.gameObject.SetActive(true);
            temp.highlight.SetActive(false);
        }

        gameObject.SetActive(false);

        ActionManager.Instance.OnActionChanged += OnActionChanged;
    }

    private void OnActionChanged(Action action)
    {
        if (m_Actions.Contains(action))
        {
            m_SelectedAction = action;

            if (OnSlotGroupActionSelected != null)
            {
                OnSlotGroupActionSelected.Invoke(m_SelectedAction);
            }
        }

        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
