using System.Collections.Generic;
using UnityEngine;

public class PointCloudAligner : MonoBehaviour
{
    public List<Vector3> AlignPointClouds(List<Vector3> pointsP, List<Vector3> pointsQ)
    {
        Vector3 centroidP = CalculateCentroid(pointsP);
        Vector3 centroidQ = CalculateCentroid(pointsQ);

        List<Vector3> translatedP = TranslatePoints(pointsP, centroidP);
        List<Vector3> translatedQ = TranslatePoints(pointsQ, centroidQ);

        // Dönüşüm tahmini (örnek: 45 derece döndürme)
        Quaternion estimatedRotation = Quaternion.Euler(0, 45, 0); // Örnek: 45 derece döndürme

        return ApplyTransformation(translatedP, estimatedRotation, centroidQ - centroidP);
    }

    Vector3 CalculateCentroid(List<Vector3> points)
    {
        if (points.Count == 0) return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (Vector3 point in points)
        {
            sum += point;
        }
        return sum / points.Count;
    }

    List<Vector3> TranslatePoints(List<Vector3> points, Vector3 translation)
    {
        List<Vector3> translatedPoints = new List<Vector3>();
        foreach (Vector3 point in points)
        {
            translatedPoints.Add(point - translation);
        }
        return translatedPoints;
    }

    List<Vector3> ApplyTransformation(List<Vector3> points, Quaternion rotation, Vector3 translation)
    {
        List<Vector3> transformedPoints = new List<Vector3>();
        foreach (Vector3 point in points)
        {
            transformedPoints.Add(rotation * point + translation);
        }
        return transformedPoints;
    }
}
