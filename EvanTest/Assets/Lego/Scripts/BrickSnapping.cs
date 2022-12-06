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
    public float snapRadius = 0.1f;
    [SerializeField]
    public Mesh snapHighlightMesh;
    [SerializeField]
    public Material snapHighlightMaterial;

    private Grabbable grabComponent;
    private List<Tuple<GameObject, GameObject>> snapPointsColliding = new List<Tuple<GameObject, GameObject>>();

    private void Start()
    {
        grabComponent = GetComponent<Grabbable>();
        ParameterSnapPoints();
    }

    void Update()
    {
        VerifyIfIsHeld();
        if (!grabComponent.BeingHeld)
        {
            snapPointsColliding.Clear();
        }
        if (snapPointsColliding.Count > 0)
        {
            //Snap();
        }
    }

    void Snap(List<Tuple<Transform, Transform>> snapPoints)
    {
        if (snapPoints.Count > 0) return;
        if (snapPoints.Count == 1)
        {
            // TODO
            return;
        }

        foreach (var pairOfSnapPoints in snapPoints)
        {

        }

        Transform firstCubeSnapPoint1 = snapPoints[0].Item1;
        Transform firstCubeSnapPoint2 = snapPoints[1].Item1;
        Transform secondCubeSnapPoint1 = snapPoints[0].Item2;
        Transform secondCubeSnapPoint2 = snapPoints[1].Item2;
        transform.position = secondCubeSnapPoint1.position + (transform.position - firstCubeSnapPoint1.position);
        transform.rotation = secondCubeSnapPoint1.GetComponentInParent<BrickSnapping>().transform.rotation;
    }

    void ParameterSnapPoints()
    {
        snapPoints = GameObject.FindGameObjectsWithTag("SnapPoint").Where(r => r.transform.IsChildOf(gameObject.transform)).ToList();
        foreach (var snapPoint in snapPoints)
        {
            var snapPointController = snapPoint.AddComponent<SnapPoint>();
            snapPointController.ParameterSnapPoint(snapHighlightMesh, snapHighlightMaterial, snapRadius);
        }
    }

    void CreateSphereMesh()
    {
        var meshBuilder = new MeshBuilder();

        // Set the number of segments and rings for the sphere
        const int segments = 64;
        const int rings = 32;

        // Create the vertices and normals for the sphere
        for (int y = 0; y <= rings; y++)
        {
            float v = y / (float)rings;
            float theta = v * Mathf.PI;

            for (int x = 0; x <= segments; x++)
            {
                float u = x / (float)segments;
                float phi = u * Mathf.PI * 2;

                Vector3 vertex = new Vector3(
                    radius * Mathf.Sin(theta) * Mathf.Cos(phi),
                    radius * Mathf.Cos(theta),
                    radius * Mathf.Sin(theta) * Mathf.Sin(phi)
                );
                Vector3 normal = (vertex - Vector3.zero).normalized;

                meshBuilder.Vertices.Add(vertex);
                meshBuilder.Normals.Add(normal);
            }
        }
        // Create the triangles for the sphere
        for (int y = 0; y < rings; y++)
        {
            for (int x = 0; x < segments; x++)
            {
                int v0 = y * (segments + 1) + x;
                int v1 = v0 + segments + 1;
                int v2 = v0 + 1;
                int v3 = v1 + 1;

                meshBuilder.AddTriangle(v0, v1, v2);
                meshBuilder.AddTriangle(v2, v1, v3);
            }
        }

        // Create the mesh
        var mesh = meshBuilder.CreateMesh();
        mesh.name = "Sphere";

        return mesh;
    }

    void VerifyIfIsHeld()
    {
        if (grabComponent.BeingHeld)
        {
            snapPointsColliding.Clear();
            foreach (var snapPoint in snapPoints)
            {
                Collider[] collidedSnapPoints = Physics.OverlapSphere(snapPoint.transform.position, snapRadius)
                    .Where(r => r.tag == "SnapPoint" && r.gameObject != snapPoint.gameObject)
                    .ToArray();
                var collider = collidedSnapPoints.FirstOrDefault();
                if (!collider) continue;
                snapPointsColliding.Add(new Tuple<GameObject, GameObject>(snapPoint, collider.gameObject));
                collider.GetComponent<SnapPoint>().IsCollidingWithSnapPoint = true;
                snapPoint.GetComponent<SnapPoint>().IsCollidingWithSnapPoint = true;
            }
        }
    }
}
