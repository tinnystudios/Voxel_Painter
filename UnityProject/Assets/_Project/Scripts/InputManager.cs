﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager> {

    public delegate void InputDelegate();
    public static event InputDelegate OnClickDown = delegate { };

    public InputConfig Config;

    void Update() {

        if (Input.GetMouseButtonDown(0))
        {
            if (OnClickDown != null)
                OnClickDown.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            if(Input.GetKey(KeyCode.LeftShift))
                HistoryManager.Instance.Redo();
            else
                HistoryManager.Instance.Undo();
        }

        if (Input.GetKeyUp(KeyCode.C))
            ColorManager.Instance.ToggleColorInfoMenu();
    }
}

public static class ActionExtensions
{
    public static void SafeInvoke(this System.Action action)
    {
        if (action != null)
            action.Invoke();
    }

    public static void SafeInvoke<T>(this System.Action<T> action, T t)
    {
        if (action != null)
            action.Invoke(t);
    }
}