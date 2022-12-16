using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    #region Variables
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private SphereCollider sphereCollider;
    private Transform parentBrick;
    private SnapPointPolarity polarity;
    private SnapPointState state;
    private float snapRadius = .1f;
    protected SnapPoint snappedTo;
    #endregion

    #region Properties
    public SnapPointState State 
    {
        get { return this.state; }
        set
        {
            state = value;
            switch (value)
            {
                case SnapPointState.Snapped:
                    meshRenderer.enabled = false;
                    break;
                case SnapPointState.NotSnapped:
                    TemporaryUnSnap();
                    meshRenderer.enabled = false;
                    break;
                case SnapPointState.CanBeSnapped:
                    meshRenderer.enabled = true;
                    break;
                default:
                    break;
            }
        } 
    }
    #endregion

    private void Update()
    {
        var collider = Physics.OverlapSphere(transform.position, snapRadius)
            .Where(r => SnapPoint.CanBeSnapped(this, r)).FirstOrDefault();
        if (collider != null)
        {
            State = SnapPointState.CanBeSnapped;
        }
        meshRenderer.enabled = collider == null ? false : true;
    }

    public void ParameterSnapPoint(Transform parent, Mesh mesh, Material material, float snapRadius, SnapPointPolarity snapPointPolarity)
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        meshRenderer.receiveShadows = false;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        sphereCollider.radius = snapRadius;
        parentBrick = parent;
        transform.localScale *= (snapRadius*5);
        polarity = snapPointPolarity;
        State = SnapPointState.NotSnapped;
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
            var isOpposite = snapPoint.polarity != otherSnapPoint.polarity;
            return beingHeld && isNotSameCollider && isSnapPoint && isNotFromSameBrick && isFree && isOpposite;
        }
        catch
        {
            return false;
        }
    }
}

public enum SnapPointPolarity
{
    Positive,
    Negative
}

public enum SnapPointState
{
    Snapped,
    NotSnapped,
    CanBeSnapped
}