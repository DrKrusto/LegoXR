using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyLego()
    {
        var hands = GameObject.FindGameObjectsWithTag("Hand");
        List<GameObject> destroyLego = new();
        foreach(GameObject hand in hands)
        {
            destroyLego.Add(hand.transform.parent.gameObject);
        }
        foreach (GameObject lego in destroyLego)
        {
            if(lego.tag == "Lego")
            {
                Destroy(lego);
            }
        }
        
    }

    public void DesactivateUI()
    {
        this.gameObject.SetActive(false);
    }

    public void ActivateUI()
    {
        this.gameObject.SetActive(true);
    }
}
