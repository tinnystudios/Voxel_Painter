using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrefabContextMenu : MonoBehaviour
{
    public string Id { get; private set; }
    public GameObject Group;

    private void Awake()
    {
        SetState(false);

        SymbolButton.OnClick += OnSymbolButtonClicked;
    }

    private void OnSymbolButtonClicked(SymbolButton symButton)
    {
        SetContext(symButton.Model.Id);
    }

    public void SetContext(string id)
    {
        Id = id;
        SetState(true);
    }

    public void SetState(bool state)
    {
        Group?.SetActive(state);
    }

    private void Update()
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedObj == null)
            {
                SetState(false);
            }
            else
            {
                var button = selectedObj.GetComponent<SymbolButton>();
                if (button != null)
                {
                    // SetContext(button.Model.Id);
                }
                else
                {
                    SetState(false);
                }
            }
        }
    }

    public void RemoveContext()
    {
        Id = null;
    }

    public void Apply()
    {
        SymbolManager.Instance.Save(Id);
    }

    public void Delete()
    {
        SymbolManager.Instance.Delete(Id);
    }
}
