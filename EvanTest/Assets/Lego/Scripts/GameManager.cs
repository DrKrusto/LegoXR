using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UiLego UiLego;
    public Ui Ui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenCloseUiLego();
        OpenUiBin();
    }

    void OpenCloseUiLego()
    {
        if(Input.GetButtonDown("Oculus_CrossPlatform_Button2"))
        {
            UiLego.DesActivateUI();
        }
    }

    void OpenUiBin()
    {
        var isGrabbed = false;
        var hands = GameObject.FindGameObjectsWithTag("Hand");
        List<GameObject> allLego = new();
        foreach (GameObject hand in hands)
        {
            allLego.Add(hand.transform.parent.gameObject);
        }
        foreach (GameObject lego in allLego)
        {
            if (lego.tag == "Lego")
            {
                isGrabbed = true;
            }
        }
        if (isGrabbed)
        {
            Ui.ActivateUI();
        }
        else
        {
            Ui.DesactivateUI();
        }
    }
}
