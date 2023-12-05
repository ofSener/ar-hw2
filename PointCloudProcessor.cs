using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudProcessor : MonoBehaviour
{
    public PointCloudLoader pointCloudLoader; // Assign this through the Inspector
    public PointCloudAligner pointCloudAligner; // Assign this through the Inspector

    private List<Vector3> alignedPointCloud; // Store the aligned point cloud

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to wait for the point cloud data to be loaded
        StartCoroutine(ProcessPointCloudsWhenReady());
    }

    IEnumerator ProcessPointCloudsWhenReady()
    {
        // Wait until the PointCloudLoader has finished loading the data
        while (!IsDataReady())
        {
            yield return null; // Wait until the next frame and check again
        }

        // Now that the data is ready, we can access it
        List<Vector3> pointCloud1 = pointCloudLoader.GetPointCloud1();
        List<Vector3> pointCloud2 = pointCloudLoader.GetPointCloud2();

        // Check if the point clouds are valid and have points
        if (pointCloud1 != null && pointCloud1.Count > 0 && pointCloud2 != null && pointCloud2.Count > 0)
        {
            // Call the alignment method from PointCloudAligner
            alignedPointCloud = pointCloudAligner.AlignPointClouds(pointCloud1, pointCloud2);

            // Handle the aligned point cloud
            HandleAlignedPointCloud(alignedPointCloud);
        }
        else
        {
            Debug.LogError("PointCloud data is not available.");
        }
    }

    // This method should be added to your PointCloudLoader script
    // It should return true when the data is ready to be accessed
    private bool IsDataReady()
    {
       
        return true;
    }

    // Handle the aligned point cloud here
    void HandleAlignedPointCloud(List<Vector3> alignedPoints)
{
    foreach (Vector3 point in alignedPoints)
    {
        // Create a sphere GameObject at the position of the aligned point
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = point;
        sphere.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Adjust the size of the spheres as needed
        sphere.GetComponent<Renderer>().material.color = Color.green; // Set the color of the spheres
    }
}

}
