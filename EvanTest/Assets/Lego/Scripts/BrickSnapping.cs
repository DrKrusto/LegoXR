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
        if (snapPointsColliding.Count > 0) return;
        if (snapPointsColliding.Count == 1)
        {
            // TODO
            return;
        }

        foreach (var pairOfSnapPoints in snapPointsColliding)
        {

        }

        Transform firstCubeSnapPoint1 = snapPointsColliding[0].Item1.transform;
        Transform firstCubeSnapPoint2 = snapPointsColliding[1].Item1.transform;
        Transform secondCubeSnapPoint1 = snapPointsColliding[0].Item2.transform;
        Transform secondCubeSnapPoint2 = snapPointsColliding[1].Item2.transform;
        transform.position = secondCubeSnapPoint1.position + (transform.position - firstCubeSnapPoint1.position);
        transform.rotation = secondCubeSnapPoint1.GetComponentInParent<BrickSnapping>().transform.rotation;
    }

    void ParameterSnapPoints()
    {
        snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint").Where(r => r.transform.IsChildOf(gameObject.transform)).ToList();
        foreach (var snapPoint in snapPoints)
        {
            var snapPointController = snapPoint.AddComponent<SnapPoint>();
            snapPointController.ParameterSnapPoint(transform, snapHighlightMesh, snapHighlightMaterial, snapRadius);
        }
    }

    void HoldingBrick()
    {
        if (grabComponent.BeingHeld)
        {
            snapPointsColliding.Clear();
            foreach (var snapPoint in snapPoints.Select(r => r.GetComponent<SnapPoint>()))
            {
                var collidedSnapPoint = Physics.OverlapSphere(snapPoint.transform.position, snapRadius)
                    .Where(r => r.tag == "SnapPoint" && r.gameObject != snapPoint.gameObject && r.transform.parent != transform.parent)
                    .Select(r => r.GetComponent<SnapPoint>())
                    .FirstOrDefault();
                if (!collidedSnapPoint) continue;
                snapPoint.SnapToSnapPoint(collidedSnapPoint);
                snapPointsColliding.Add(new Tuple<SnapPoint, SnapPoint>(snapPoint, collidedSnapPoint));
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
}
