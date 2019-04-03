using UnityEngine;

public static class TransformUtils
{
    public static Vector3 Center(Transform[] transforms)
    {
        var totalX = 0f;
        var totalY = 0f;
        var totalZ = 0f;

        foreach (var t in transforms)
        {
            totalX += t.position.x;
            totalY += t.position.y;
            totalZ += t.position.z;
        }

        var centerX = totalX / transforms.Length;
        var centerY = totalY / transforms.Length;
        var centerZ = totalZ / transforms.Length;

        return new Vector3(centerX, centerY, centerZ);
    }

    public static Vector3 CenterLocal(Transform[] transforms)
    {
        var totalX = 0f;
        var totalY = 0f;
        var totalZ = 0f;

        foreach (var t in transforms)
        {
            totalX += t.localPosition.x;
            totalY += t.localPosition.y;
            totalZ += t.localPosition.z;
        }

        var centerX = totalX / transforms.Length;
        var centerY = totalY / transforms.Length;
        var centerZ = totalZ / transforms.Length;

        return new Vector3(centerX, centerY, centerZ);
    }
}
