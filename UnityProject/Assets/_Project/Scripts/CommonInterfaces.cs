using UnityEngine;

public interface IAction {
    bool Use();
    void Select();
    void Deselect();
    void Undo();
    void Redo();
    GameObject gameObject { get; }
}

public interface ISelectable {
    bool IsSelected { get; }
    void Select();
    void Deselect();
    void Hover();
    void HoverExit();
    GameObject gameObject { get; }
}

[System.Serializable]
public class Selectable : IUnifiedContainer<ISelectable> { }