using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SnapGrid : MonoBehaviour
{
    private Grid grid;

    [HideInInspector]
    public Vector3[] snapPoints;

    [SerializeField]
    private int rows;
    [SerializeField]
    private int columns;
    [SerializeField]
    private GameObject objectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        snapPoints = ComputeGridPoints(transform.position, transform.localScale.x, transform.localScale.z, rows, columns, 0);
        foreach (var snapPoint in snapPoints)
        {
            Instantiate(objectToSpawn, transform);
        }
    }

    public Vector3[] ComputeGridPoints(Vector3 surfacePosition, float surfaceWidth, float surfaceHeight, int numRows, int numColumns, float angle)
    {
        // Determine the center point of the surface
        Vector3 centerPoint = surfacePosition + new Vector3(surfaceWidth / 2, 0, surfaceHeight / 2);

        // Compute the side length of the smaller squares
        float squareWidth = surfaceWidth / numColumns;
        float squareHeight = surfaceHeight / numRows;

        // Compute the coordinates of the points at the corners of the smaller squares
        Vector3[] points = new Vector3[numRows * numColumns * 4];
        int pointIndex = 0;
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                Vector3 squareCenter = centerPoint + new Vector3(-surfaceWidth / 2 + (col + 0.5f) * squareWidth, 0, -surfaceHeight / 2 + (row + 0.5f) * squareHeight);
                points[pointIndex++] = squareCenter + new Vector3(-squareWidth / 2, 0, -squareHeight / 2);
                points[pointIndex++] = squareCenter + new Vector3(squareWidth / 2, 0, -squareHeight / 2);
                points[pointIndex++] = squareCenter + new Vector3(squareWidth / 2, 0, squareHeight / 2);
                points[pointIndex++] = squareCenter + new Vector3(-squareWidth / 2, 0, squareHeight / 2);
            }
        }

        // Apply rotation to the points if necessary
        if (angle != 0)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = rotation * points[i];
            }
        }

        return points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}