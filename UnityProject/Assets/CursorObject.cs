using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;

        var pos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = pos;
    }
}
