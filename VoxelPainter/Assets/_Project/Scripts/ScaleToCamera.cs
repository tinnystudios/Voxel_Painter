using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    public Camera MainCamera;
    public float Damp = 0.015f;

    private void Update()
    {
        var dist = Vector3.Distance(MainCamera.transform.position, transform.position);
        transform.localScale = Vector3.one * Damp * dist;
    }
}

