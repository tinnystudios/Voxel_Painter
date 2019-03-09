using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace core
{
    public class ActionManager : Singleton<ActionManager>
    {
        public Action selectedAction;
        public List<Action> actions = new List<Action>();
        public delegate void ActionManagerDelegate(Action action);
        public event ActionManagerDelegate OnActionChanged;
        
        void Awake() {
            var iActions = GetComponentsInChildren<IAction>();

            foreach (IAction action in iActions) {
                actions.Add(new Action(action));
                action.Deselect();
            }

            SelectAction(actions[0]);
        }

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            foreach (var action in actions)
            {
                var shortKey = GetShortKey(action);
                if (shortKey != null && Input.GetKeyDown(shortKey.Key))
                {
                    SetSelectAction(action);
                }
            }


            var toolManager = ToolManager.Instance;

            for (int i = 1; i < toolManager.slots.Count+1; i++) {
                int index = i - 1;
                var element = toolManager.slots[index].action;

                if (Input.GetKeyDown(i.ToString()))
                {
                    SetSelectAction(element);
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                if (selectedAction != null)
                {
                    if (!Input.GetKey(KeyCode.LeftAlt))
                        Use(selectedAction);
                }
            }
        }

        public void Undo()
        {

        }

        public void SetSelectAction(Action action)
        {
            if (selectedAction.Result != null && selectedAction != action)
            {
                selectedAction.Result.Deselect();
            }

            if (selectedAction.Result != null && selectedAction != action || selectedAction == null)
            {
                SelectAction(action);
            }
        }

        public void SelectAction(Action iAction) {

            if (selectedAction.Result != null && selectedAction != iAction)
                selectedAction.Result.Deselect();

            selectedAction = iAction;
            selectedAction.Result.Select();
            Debug.Log("[" + selectedAction.Result.ToString() + "]");
            if (OnActionChanged != null)
                OnActionChanged.Invoke(iAction);
        }

        public void Use(Action action) {
            if(action.Result.Use())
                HistoryManager.Instance.AddAction(action);
        }

        public IShortKey GetShortKey(Action action)
        {
            var shortkey = action.Result as IShortKey;
            if (shortkey == null) return null;
            return shortkey;
        }
    }
}