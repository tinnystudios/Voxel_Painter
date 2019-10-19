using UnityEngine;
using UnityEngine.EventSystems;

public class SaveAs : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ApplicationManager.Instance.SaveAs();
    }
}
