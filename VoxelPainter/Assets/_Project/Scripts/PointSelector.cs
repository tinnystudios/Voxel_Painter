using UnityEngine;

[ExecuteInEditMode]
public class PointSelector : MonoBehaviour
{
    public MeshFilter MeshFilter;

    private void OnDrawGizmos()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform != transform)
                return;

            var point = NearestVertexTo(hit.point, MeshFilter.mesh);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point, 0.2F);

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(hit.point, 0.2F);
        }
    }

    public Vector3 NearestVertexTo(Vector3 point, Mesh mesh)
    {
        point = transform.InverseTransformPoint(point);

        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;

        // scan all vertices to find nearest
        foreach (Vector3 vertex in mesh.vertices)
        {
            Vector3 diff = point - vertex;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }
        }

        // convert nearest vertex back to world space
        return transform.TransformPoint(nearestVertex);
    }
}
