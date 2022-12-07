using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UiLego : MonoBehaviour
{
    public GameObject[] Lego;
    public Material[] LegoMaterial;
    private int index = 0;

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
    }

    void ChangePage(int index)
    {
        var positionLego = GameObject.FindGameObjectsWithTag("Spawn").Select(r => r.gameObject.transform).ToArray();
        var legoPrefab = GameObject.FindGameObjectsWithTag("LegoPrefab");
        foreach (var lego in legoPrefab)
        {
            Destroy(lego);
        }

        for (int i = index * 15; i < (index + 1) * 15; i++)
        {
            Instantiate(Lego[i], positionLego[i - index * 15].position, Quaternion.Euler(new Vector3(0, 90, 0)), positionLego[i - index * 15]);
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

    public void ActivateUI()
    {
        this.gameObject.SetActive(true);
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
}