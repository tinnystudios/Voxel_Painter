using UnityEngine;
using UnityEngine.EventSystems;

public class Save : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ApplicationManager.Instance.Save();
    }
}
