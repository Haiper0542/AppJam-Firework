using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    #region Variables
    
    public bool isEquiped = false;

    [SerializeField] private GameObject CrossHair;
    [SerializeField] private Transform HeadTr;
    [SerializeField] private Transform EquipTr;
    [SerializeField] private Transform LargeEquipTr;
    
    [SerializeField] protected GameObject Hand_IdleModel;
    [SerializeField] protected GameObject Hand_GrabModel;
    [SerializeField] protected GameObject Hand_LargeGrabModel;
    
    private Ray ray;
    private RaycastHit hit;

    private GameObject EquipableObj;
    private bool isLock = false;

    #endregion

    #region Components
    
    private SteamVR_TrackedObject trackedObj;

    public SteamVR_Controller.Device controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    
    private Transform tr;

    #endregion
    
    #region Life Cycle Method

    private void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        tr = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!isLock)
        {
            if (controller.GetHairTrigger())
            {
                Hand_IdleModel.SetActive(false);
                Hand_GrabModel.SetActive(true);
                Hand_LargeGrabModel.SetActive(false);
            }
            else
            {
                Hand_IdleModel.SetActive(true);
                Hand_GrabModel.SetActive(false);
                Hand_LargeGrabModel.SetActive(false);
            }
        }
        
        if(!isEquiped)
            Hand_None();
        else
            Hand_Equip();
        
        
        if (controller.GetHairTriggerDown())
        {
            if (!isEquiped && EquipableObj != null)
            {
                EquipFirework();
            }
            else if (isEquiped)
            {
                if (EquipableObj.GetComponent<Firework>().InFire)
                {
                    EquipableObj.GetComponent<Firework>().BurnFirework();
                }
                else if (EquipableObj.GetComponent<Firework>().State == FireworkState.Off)
                {
                    UnLockHand();
                    EquipableObj.GetComponent<Firework>().Reset();
                    EquipableObj = null;
                    isEquiped = false;
                }
            }
        }
    }

    #endregion

    #region Other Methods

    private void ActiveCrossHair(Vector3 positon)
    {
        if (!CrossHair.activeInHierarchy)
        {
            CrossHair.SetActive(true);
        }
        CrossHair.GetComponent<Transform>().position = positon;
        CrossHair.transform.LookAt(HeadTr);
    }

    private void DisableCrossHair()
    {
        if(isEquiped)
            CrossHair.SetActive(false);
    }
    
    private void Hand_None()
    {
        ray = new Ray(tr.position, tr.forward * 10f);
        Debug.DrawRay(tr.position, tr.forward * 10f, Color.green);

        if (Physics.Raycast(ray, out hit))
        {
            ActiveCrossHair(hit.point);
            if (hit.collider.CompareTag("Firework"))
            {
                if (hit.collider.GetComponent<Firework>().State == FireworkState.Item)
                {
                    EquipableObj = hit.collider.gameObject;
                }
            }
            
        }
        else
        {
            DisableCrossHair();
            EquipableObj = null;
        }
    }

    private void Hand_Equip()
    {
        
    }

    private void EquipFirework()
    {
        DisableCrossHair();

        if (EquipableObj.GetComponent<Firework>().Type == FireworkType.Speaker)
        {
            LockHand(HandState.LargeGrab);
            EquipableObj.transform.SetParent(LargeEquipTr);
        }
        else
        {
            LockHand(HandState.Grab);
            EquipableObj.transform.SetParent(EquipTr);
        }
        
        EquipableObj.transform.localPosition = Vector3.zero;
        EquipableObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        
        EquipableObj.GetComponent<Firework>().EquipFirework();
        
        isEquiped = true;
    }
    
    public void LockHand(HandState state)
    {
        isLock = true;
        
        Hand_IdleModel.SetActive(false);
        Hand_GrabModel.SetActive(false);
        Hand_LargeGrabModel.SetActive(false);
        
        switch (state)
        {
            case HandState.Idle:
                Hand_IdleModel.SetActive(true);
                break;
            case HandState.Grab:
                Hand_GrabModel.SetActive(true);
                break;
            case HandState.LargeGrab:
                Hand_LargeGrabModel.SetActive(true);
                break;
        }
    }

    public void UnLockHand()
    {
        isLock = false;
    }

    #endregion
}
