using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;
    private Transform parentBrick;
    private float snapRadius = .1f;
    protected SnapPoint snappedTo;

    private void Update()
    {
        var collider = Physics.OverlapSphere(transform.position, snapRadius).Where(r => CanBeSnapped(r)).FirstOrDefault();
        meshRenderer.enabled = CanBeSnapped(collider);
    }

    public void ParameterSnapPoint(Transform parent, Mesh mesh, Material material, float snapRadius)
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        meshRenderer.enabled = false;
        sphereCollider.radius = 1;
        parentBrick = parent;
        transform.localScale *= snapRadius;
    }

    public void SnapToSnapPoint(SnapPoint snapPoint)
    {
        snappedTo = snapPoint;
        snapPoint.snappedTo = this;
    }

    public void UnSnap()
    {
        snappedTo.snappedTo = null;
        snappedTo = null;
    }

    private bool CanBeSnapped(Collider other)
    {
        try
        {
            var otherParent = other.gameObject.GetComponentInParent<SnapPoint>().parentBrick;
            var beingHeld = otherParent.GetComponent<Grabbable>().BeingHeld || parentBrick.GetComponent<Grabbable>().BeingHeld;
            var isNotSameCollider = other.transform != transform;
            var isSnapPoint = other.tag == "SnapPoint";
            var isNotFromSameBrick = otherParent != parentBrick;
            return beingHeld && isNotSameCollider && isSnapPoint && isNotFromSameBrick;
        }
        catch
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanBeSnapped(other)) meshRenderer.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (CanBeSnapped(other)) meshRenderer.enabled = false;
    }
}
