using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using BNG;
using System;
using System.Runtime.ConstrainedExecution;

public class BrickSnapping : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public List<SnapPoint> snapPoints;

    [SerializeField]
    public float snapRadius;
    [SerializeField]
    public Mesh snapHighlightMesh;
    [SerializeField]
    public Material snapHighlightMaterial;

    private Grabbable grabComponent;
    #endregion

    #region Properties
    public List<SnapPoint> SnapPointsColliding { get; }
    #endregion

    private void Start()
    {
        grabComponent = GetComponent<Grabbable>();
        ParameterSnapPoints();
    }

    void Update()
    {
        HoldingBrick();
    }

    //void Snap()
    //{
    //    GetComponent<Rigidbody>().isKinematic = true;
    //    GetComponent<Collider>().enabled = false;
    //    var collidedSnapPointsPositions = snapPoints.Select(p => p.SnappedTo.transform.position).ToArray();
    //    var otherBrick = snapPoints.Where(p => p.State == SnapPointState.CanBeSnapped).First().ParentBrick;
    //    var position = FindMiddleVector(collidedSnapPointsPositions);
    //    transform.rotation = otherBrick.rotation;
    //    transform.position = position;
    //    gameObject.transform.SetParent(otherBrick);
        
    //}

    void ParameterSnapPoints()
    {
        snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint")
            .Where(r => r.transform.IsChildOf(gameObject.transform))
            .Select(r => r.AddComponent<SnapPoint>())
            .ToList();
        foreach (var snapPoint in snapPoints)
        {
            var snapPointType = snapPoint.transform.parent.tag == "SnapPointPositive" ? SnapPointPolarity.Positive : SnapPointPolarity.Negative;
            snapPoint.ParameterSnapPoint(transform, snapHighlightMesh, snapHighlightMaterial, snapRadius, snapPointType);
        }
    }

    void HoldingBrick()
    {
        if (!grabComponent.BeingHeld && grabComponent.LastDropTime >= Time.time - 0.1f)
        {
            var canBeSnappedPoints = snapPoints.Where(p => p.State == SnapPointState.CanBeSnapped).ToArray();
            if (canBeSnappedPoints.Count() <= 0) return;
            foreach (var snapPoint in canBeSnappedPoints)
            {
                snapPoint.State = SnapPointState.Snapped;
            }
            var newBrickPosition = FindMiddleVector(canBeSnappedPoints.Select(p => p.SnappedTo.transform.position).ToArray());
            var otherBrick = canBeSnappedPoints.First().SnappedTo.ParentBrick;
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.rotation = otherBrick.rotation;
            Bounds bounds = GetComponentInChildren<Renderer>().bounds;
            Vector3 size = bounds.size;
            transform.position = new Vector3(newBrickPosition.x, newBrickPosition.y + size.y/2, newBrickPosition.z);
            transform.SetParent(otherBrick);
        }
    }

    Vector3 FindMiddleVector2(Vector3[] points)
    {
        Vector3 totalPoints = new();
        foreach (var point in points)
        {
            totalPoints += point;
        }
        return totalPoints / points.Length;
    }

    Vector3 FindMiddleVector(Vector3[] points)
    {
        float maxDistance = 0;
        (Vector3, Vector3) twoVectors = new();
        foreach (var firstPoint in points)
        {
            foreach (var secondPoint in points)
            {
                if (firstPoint == secondPoint) continue;
                float distance = Vector3.Distance(firstPoint, secondPoint);
                if (maxDistance < distance)
                {
                    maxDistance = distance;
                    twoVectors = (firstPoint, secondPoint);
                }
            }
        }
        return (twoVectors.Item1 + twoVectors.Item2) / 2;
    }
}
