using UnityEngine;

public class ToolTip : MonoBehaviour, IToolTip
{
    [TextArea]
    public string m_Description;

    public string Description
    {
        get
        {
            return m_Description;
        }
    }
}

//The tool tip information
public interface IToolTip
{
    string Description {get;}
}

//The interface that will show the tool tip
public interface IToolTipDisplay
{
    IToolTip ToolTip { get; }
}
