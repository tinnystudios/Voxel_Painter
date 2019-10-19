using UnityEngine;
using UnityEngine.EventSystems;

public class Load : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ApplicationManager.Instance.Load();
    }
}