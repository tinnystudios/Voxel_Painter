using UnityEngine;
using UnityEngine.EventSystems;

public class SaveAsObj : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ApplicationManager.Instance.SaveObjAs();
    }
}