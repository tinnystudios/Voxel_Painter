namespace App
{
    using UnityEngine;

    public static class TransformExtensions
    {
        public static Vector3 NearestVertexTo(this Transform transform, Vector3 point, Mesh mesh)
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
}
