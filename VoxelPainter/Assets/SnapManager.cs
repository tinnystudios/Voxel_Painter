public class SnapManager : Singleton<SnapManager>
{
    public SnapSettings Settings = new SnapSettings();

    public void SetSnapSize(string sizeString)
    {
        SetSnapSize(float.Parse(sizeString));
    }

    public void SetSnapSize(float size)
    {
        Settings.GridSize = size;
    }

    public void SnapType(ESnapType type)
    {
        Settings.Type = type;
    }
}

public class SnapSettings
{
    public ESnapType Type = ESnapType.Grid;
    public float GridSize = 1.0F;
}

public enum ESnapType
{
    None,
    Grid,
    Point
}
