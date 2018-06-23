using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using Action = core.Action;

public class SlotGroup : MonoBehaviour {

    public SlotButton m_Template;
    public List<Action> m_Actions;

    private void Awake()
    {
        m_Template.gameObject.SetActive(false);

        foreach (var action in m_Actions)
        {
            var temp = Instantiate(m_Template,transform);
            temp.Init(action);
            temp.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
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
