using UnityEngine;
using UnityEngine.EventSystems;

public class AddSymbolButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SymbolManager.Instance.Save();
    }
}
