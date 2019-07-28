using core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionGroup : MonoBehaviour
{
    public List<Action> Actions;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            // Toggle
            var selectedAction = ActionManager.Instance.selectedAction;

            if (Actions.Any(x => x.Object == selectedAction.Object))
            {
                var index = Actions.FindIndex(x => x.Object == selectedAction.Object);
                var newIndex = index + 1;

                if (index >= Actions.Count - 1)
                    newIndex = 0;

                var newAction = Actions[newIndex];

                ActionManager.Instance.SelectAction(newAction);
            }
        }
    }
}
