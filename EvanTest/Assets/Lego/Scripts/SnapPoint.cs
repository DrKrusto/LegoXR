using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;
    private bool isCollidingWithSnapPoint;

    public bool IsCollidingWithSnapPoint { 
        get { return isCollidingWithSnapPoint; } 
        set
        {
            isCollidingWithSnapPoint = value;
            meshRenderer.enabled = value;
        } 
    }

    public void ParameterSnapPoint(Mesh mesh, Material material, float snapRadius)
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        IsCollidingWithSnapPoint = true;
        meshFilter.mesh = mesh; // should do procedurally according to radius!
        meshRenderer.material = material;
        sphereCollider.radius = snapRadius;
    }
}
