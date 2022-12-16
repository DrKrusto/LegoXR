using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BNG;

public class UiLego : MonoBehaviour
{
    public GameObject[] Lego;
    public Material[] LegoMaterial;
    private int index = 0;
    public Transform PlayerTransform;
    public float RotateSpeed;
    public int Index 
    { 
        get { return index; }
        set { 
            index = value;
            ChangePage(index);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangePage(0);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] LegoPrefab = GameObject.FindGameObjectsWithTag("LegoPrefab");
        foreach (var lego in LegoPrefab)
        {
            lego.transform.Rotate(0,RotateSpeed,0);
        }
    }

    void ChangePage(int index)
    {
        var positionLego = GameObject.FindGameObjectsWithTag("Spawn").Select(r => r.gameObject.transform).ToArray();
        GameObject[] LegoPrefab = GameObject.FindGameObjectsWithTag("LegoPrefab");
        foreach (var lego in LegoPrefab)
        {
            Destroy(lego);
        }

        for (int i = index * 15; i < (index + 1) * 15; i++)
        {
            Instantiate(Lego[i], positionLego[i - index * 15].position, Quaternion.Euler(new Vector3(0, 90, 0)), positionLego[i - index * 15]);
        }
        LegoPrefab = GameObject.FindGameObjectsWithTag("LegoPrefab");
        foreach (var lego in LegoPrefab)
        {
            lego.GetComponent<Rigidbody>().useGravity = false;
        }

    }

    public void IndexPlus()
    {
        if (Index == 8)
        {
            Index = 0;
        }
        else
        {
            Index++;
        }
    }

    public void IndexMoins()
    {
        if(Index == 0)
        {
            Index = 8;
        } else
        {
            Index--;
        }
    }

    public void DesActivateUI()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void DesactivateUI()
    {
        this.gameObject.SetActive(false);
    }

    public void SetMaterial(int color)
    {
        foreach (var lego in Lego)
        {
            lego.GetComponent<MeshRenderer>().material = LegoMaterial[color];
        }
        ChangePage(Index);

    }

    public void HandInstantiate(int pos)
    {
        //primaryGrabOffset.GetComponent<GrabPoint>().HandPose
        GameObject newLegoGO;
        newLegoGO = Instantiate(Lego[pos+Index*15],PlayerTransform.position + PlayerTransform.forward, Quaternion.identity);
        newLegoGO.tag = "Lego";
        newLegoGO.GetComponent<MeshCollider>().enabled = true;
        var grab = newLegoGO.GetComponent<Grabbable>();
        grab.enabled = true;
        newLegoGO.GetComponent<Rigidbody>().isKinematic = true;
        newLegoGO.transform.localScale = new Vector3(10, 10, 10);
  
        DesactivateUI();
    }
}