using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Config")]
public class InputConfig : ScriptableObject
{
    public KeyCode Pan = KeyCode.Q | KeyCode.Space;
    public KeyCode Move = KeyCode.W;
    public KeyCode Scale = KeyCode.R;
}
