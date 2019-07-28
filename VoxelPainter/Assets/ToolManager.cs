using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using System.Linq;

public class ToolManager : Singleton<ToolManager> {
    public List<SlotContainer> slots;
    public List<SlotButton> slotButtons;

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
            if (slot.Actions.Any(x => x.Object == selectedAction.Object))
            {
                slot.slotButton.Select();
            }
            else
            {
                slot.slotButton.Deselect();
            }
        }
    }

    private void Instance_OnActionChanged(Action action)
    {
        foreach (var slotButton in slotButtons)
        {
            
        }
    }

    [System.Serializable]
    public class SlotContainer {
        public core.Action action;

        // We may want to bind more than 1 action to this button
        public List<core.Action> Actions;

        public SlotButton slotButton;
        public Sprite sprite;

        public void Init() {
            slotButton.image.sprite = sprite;
            slotButton.Init(action);
        }
    }
}
