using UnityEngine;
using UnityEngine.UI;

public class TitleDisplay : MonoBehaviour
{
    public Text Text;

    public void Start()
    {
        var scene = ApplicationManager.Instance.SceneName;
        Text.text = scene;
    }
}
