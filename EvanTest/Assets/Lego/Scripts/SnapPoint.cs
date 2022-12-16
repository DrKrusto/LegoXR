using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;
    private Transform parentBrick;
    private SnapPointType snapPointType;
    private float snapRadius = .1f;
    protected SnapPoint snappedTo;

    private void Update()
    {
        var collider = Physics.OverlapSphere(transform.position, snapRadius)
            .Where(r => SnapPoint.CanBeSnapped(this, r)).FirstOrDefault();
        meshRenderer.enabled = collider == null ? false : true;
    }

    public void ParameterSnapPoint(Transform parent, Mesh mesh, Material material, float snapRadius, SnapPointType snapPointType)
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        meshRenderer.enabled = false;
        meshRenderer.receiveShadows = false;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        sphereCollider.radius = snapRadius;
        parentBrick = parent;
        transform.localScale *= (snapRadius*5);
        this.snapPointType = snapPointType;
    }

    public void TemporarySnap(SnapPoint snapPoint)
    {
        snappedTo = snapPoint;
        snapPoint.snappedTo = this;
    }

    public void TemporaryUnSnap()
    {
        snappedTo.snappedTo = null;
        snappedTo = null;
    }

    static public bool CanBeSnapped(SnapPoint snapPoint, Collider other)
    {
        try
        {
            var otherSnapPoint = other.gameObject.GetComponentInParent<SnapPoint>();
            var beingHeld = otherSnapPoint.parentBrick.GetComponent<Grabbable>().BeingHeld || snapPoint.parentBrick.GetComponent<Grabbable>().BeingHeld;
            var isNotSameCollider = other.transform != snapPoint.transform;
            var isSnapPoint = other.tag == "SnapPoint";
            var isNotFromSameBrick = otherSnapPoint.parentBrick != snapPoint.parentBrick;
            var isFree = snapPoint.snappedTo == null && otherSnapPoint.snappedTo == null;
            var isOpposite = snapPoint.snapPointType != otherSnapPoint.snapPointType;
            return beingHeld && isNotSameCollider && isSnapPoint && isNotFromSameBrick && isFree && isOpposite;
        }
        catch
        {
            return false;
        }
    }
}

public enum SnapPointType
{
    Positive,
    Negative
}