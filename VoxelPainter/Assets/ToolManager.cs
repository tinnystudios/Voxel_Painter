using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using System.Linq;

public class ToolManager : Singleton<ToolManager>
{
    public List<SlotContainer> slots;

    private void Awake()
    {
        ActionManager.Instance.OnActionChanged += Instance_OnActionChanged;

        foreach (SlotContainer slot in slots)
            slot.Init();
    }
    private void Update()
    {
        var selectedAction = ActionManager.Instance.selectedAction;

        foreach (var slot in slots)
        {
            if (slot.Actions.Any(x => x.Action.Object == selectedAction.Object))
            {
                var action = slot.Actions.FirstOrDefault(x => x.Action.Object == selectedAction.Object);
                slot.slotButton.Select(action.sprite);
            }
            else
            {
                slot.slotButton.Deselect();
            }
        }
    }

    private void Instance_OnActionChanged(Action action)
    {

    }

    [System.Serializable]
    public class SlotContainer {
        public List<ActionModel> Actions;
        public SlotButton slotButton;

        public void Init() {
            slotButton.Init(Actions[0].Action);
        }
    }

    [System.Serializable]
    public class ActionModel
    {
        public core.Action Action;
        public Sprite sprite;
    }
}
