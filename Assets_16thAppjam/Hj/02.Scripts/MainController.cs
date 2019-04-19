using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    public UIButtonCtrl nowClicked;
    public LayerMask UIMask;
    
    public Transform RController;
    public Transform LController;

    public GameObject RRay;
    public GameObject LRay;

    private bool isRight = true;

    public bool Rtrigger = false;
    public bool Ltrigger = false;

    public void RTriggerDown()
    {
        Rtrigger = true;
        if (!isRight)
        {
            isRight = true;
            RRay.SetActive(true);
            LRay.SetActive(false);
        }
    }
    public void RTriggerUp()
    {
        Rtrigger = false;
        if (!isRight)
        {
            isRight = false;
            RRay.SetActive(true);
            LRay.SetActive(false);
        }
    }

    public void LTriggerDown()
    {
        Ltrigger = true;
        if (isRight)
        {
            isRight = false;
            RRay.SetActive(false);
            LRay.SetActive(true);
        }
    }
    public void LTriggerUp()
    {
        Ltrigger = false;
        if (isRight)
        {
            isRight = false;
            RRay.SetActive(false);
            LRay.SetActive(true);
        }
    }

    private void Update()
    {
        RaycastHit hit;
        if (isRight)
        {
            Debug.DrawRay(RController.position, RController.forward);
            if (Physics.Raycast(RController.position, RController.forward, out hit, UIMask))
            {
                Debug.Log((hit.transform.name));
                nowClicked = hit.transform.GetComponent<UIButtonCtrl>();
                if (Rtrigger)
                {
                    nowClicked.Pressed();
                }
                else
                {
                    nowClicked.Highlight();
                }
            }
            else if (nowClicked != null)
            {
                nowClicked.UnHighlight();
            }
        }
        else
        {
            if (Physics.Raycast(LController.position, LController.forward, out hit, UIMask))
            {
                nowClicked = hit.transform.GetComponent<UIButtonCtrl>();
                if (Ltrigger)
                {
                    nowClicked.Pressed();
                }
                else
                {
                    nowClicked.Highlight();
                }
            }
            else if (nowClicked != null)
            {
                nowClicked.UnHighlight();
            }
        }
    }
}
