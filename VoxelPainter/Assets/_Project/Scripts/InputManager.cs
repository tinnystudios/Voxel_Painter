using System;
using UnityEngine;

public class InputManager : Singleton<InputManager> {

    public delegate void InputDelegate();
    public static event InputDelegate OnClickDown = delegate { };

    public InputConfig Config;

    public MyCamera GodMovement;
    public FPSMovement FPSMovement;
    public PlatformerMovement PlatformerMovement;

    public bool UseFPSMovement;

    public Action OnInputVertexSnap;
    public Action OnInputVertexSnapUp;

    void Update()
    {
        GodMovement.enabled = !UseFPSMovement;
        FPSMovement.enabled = UseFPSMovement;

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

        if (Input.GetKey(KeyCode.V))
            OnInputVertexSnap?.Invoke();

        if (Input.GetKeyUp(KeyCode.V))
            OnInputVertexSnapUp?.Invoke();
    }
    
    public void SetMode(BaseMovement movement)
    {
        UseFPSMovement = movement is FPSMovement || movement is PlatformerMovement;
        movement.enabled = true;

        if (!(movement is PlatformerMovement)) 
        {
            PlatformerMovement.enabled = false;
        }
    }
}

public enum EControlMode
{
    GodMode,
    Flythrough,
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