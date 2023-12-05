using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PointCloudLoader : MonoBehaviour
{
    private List<Vector3> points1;
    private List<Vector3> points2;
    private bool dataReady = false;

    // Bu yollar StreamingAssets klasörüne göre ayarlanmalıdır
    private string filePath1 = "PointCloud1.txt";
    private string filePath2 = "PointCloud2.txt";

    void Start()
    {
        StartCoroutine(LoadPointClouds());
    }

    IEnumerator LoadPointClouds()
    {
        string fullPath1 = Path.Combine(Application.streamingAssetsPath, filePath1);
        string fullPath2 = Path.Combine(Application.streamingAssetsPath, filePath2);

        string data1 = "";
        string data2 = "";

        // Dosyaları asenkron olarak yükle
        if (fullPath1.Contains("://") || fullPath1.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www1 = UnityEngine.Networking.UnityWebRequest.Get(fullPath1);
            yield return www1.SendWebRequest();
            data1 = www1.downloadHandler.text;
        }
        else
            data1 = System.IO.File.ReadAllText(fullPath1);

        if (fullPath2.Contains("://") || fullPath2.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www2 = UnityEngine.Networking.UnityWebRequest.Get(fullPath2);
            yield return www2.SendWebRequest();
            data2 = www2.downloadHandler.text;
        }
        else
            data2 = System.IO.File.ReadAllText(fullPath2);

        points1 = ExtractPointsFromText(data1);
        points2 = ExtractPointsFromText(data2);

        VisualizePoints(points1, Color.red);
        VisualizePoints(points2, Color.blue);

        SetDataReady();
    }

    public bool IsDataReady()
    {
        return dataReady;
    }

    private void SetDataReady()
    {
        dataReady = true;
    }

    public List<Vector3> GetPointCloud1()
    {
        return points1;
    }

    public List<Vector3> GetPointCloud2()
    {
        return points2;
    }

    private List<Vector3> ExtractPointsFromText(string text)
    {
        List<Vector3> points = new List<Vector3>();
        string[] lines = text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split(' ');
            if (values.Length == 3)
            {
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                float z = float.Parse(values[2]);
                points.Add(new Vector3(x, y, z));
            }
        }
        return points;
    }

    void VisualizePoints(List<Vector3> points, Color pointColor)
    {
        foreach (Vector3 point in points)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = point;
            sphere.transform.localScale = new Vector3(1f, 1f, 1f);  
            sphere.GetComponent<Renderer>().material.color = pointColor;
        }
    }
}
