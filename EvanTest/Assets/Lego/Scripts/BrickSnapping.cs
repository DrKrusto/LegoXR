using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using BNG;
using System;
using System.Runtime.ConstrainedExecution;

public class BrickSnapping : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> snapPoints;

    [SerializeField]
    public float snapRadius;
    [SerializeField]
    public Mesh snapHighlightMesh;
    [SerializeField]
    public Material snapHighlightMaterial;

    private Grabbable grabComponent;
    private List<Tuple<SnapPoint, SnapPoint>> snapPointsColliding = new();

    private void Start()
    {
        grabComponent = GetComponent<Grabbable>();
        ParameterSnapPoints();
    }

    void Update()
    {
        HoldingBrick();
    }

    void Snap()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        var collidedSnapPoints = snapPointsColliding.Select(p => p.Item2.transform.position).ToArray();
        var position = FindMiddleVector(collidedSnapPoints);
        transform.position = position;
    }

    void ParameterSnapPoints()
    {
        snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint").Where(r => r.transform.IsChildOf(gameObject.transform)).ToList();
        foreach (var snapPoint in snapPoints)
        {
            var snapPointController = snapPoint.AddComponent<SnapPoint>();
            var snapPointType = snapPoint.transform.parent.tag == "SnapPointPositive" ? SnapPointPolarity.Positive : SnapPointPolarity.Negative;
            snapPointController.ParameterSnapPoint(transform, snapHighlightMesh, snapHighlightMaterial, snapRadius, snapPointType);
        }
    }

    void HoldingBrick()
    {
        if (grabComponent.BeingHeld)
        {
            ClearSnapPoints();
            foreach (var snapPoint in snapPoints.Select(r => r.GetComponent<SnapPoint>()))
            {
                //var collidedSnapPoint = Physics.OverlapSphere(snapPoint.transform.position, snapRadius)
                //    .Where(r => SnapPoint.CanBeSnapped(snapPoint, r))
                //    .Select(r => r.GetComponent<SnapPoint>())
                //    .FirstOrDefault();
                //if (!collidedSnapPoint) continue;
                //snapPoint.TemporarySnap(collidedSnapPoint);
                //snapPointsColliding.Add(new Tuple<SnapPoint, SnapPoint>(snapPoint, collidedSnapPoint));
            }
        }
        else
        {
            if (snapPointsColliding.Count > 0)
            {
                //Snap();
            }
        }
    }

    Vector3 FindMiddleVector(Vector3[] points)
    {
        Vector3 totalPoints = new();
        foreach (var point in points)
        {
            totalPoints += point;
        }
        return totalPoints / points.Length;
    }

    void ClearSnapPoints()
    {
        foreach (var snapPoints in snapPointsColliding)
        {
            snapPoints.Item1.TemporaryUnSnap();
        }
        snapPointsColliding.Clear();
    }
}
