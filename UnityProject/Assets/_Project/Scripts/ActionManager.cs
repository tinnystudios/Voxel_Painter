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
            //var iActions = InterfaceHelper.FindObjects<IAction>();

            foreach (IAction action in iActions) {
                actions.Add(new Action(action));
            }

            SelectAction(actions[0]);
        }

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var toolManager = ToolManager.Instance;



            for (int i = 1; i < toolManager.slots.Count+1; i++) {
                int index = i - 1;
                var element = toolManager.slots[index].action;

                if (Input.GetKeyDown(i.ToString()))
                {
                    if (selectedAction != element && selectedAction != null)
                        selectedAction.Result.Deselect();

                    if (selectedAction != null && selectedAction != element || selectedAction == null)
                    {
                        SelectAction(element);
                    }
                    
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                if (selectedAction != null)
                {
                    if (!Input.GetKey(KeyCode.LeftAlt))
                        Use(selectedAction);
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                selectedAction = null;
            }
        }

        public void Undo()
        {

        }

        public void SelectAction(Action iAction) {
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
    }
}