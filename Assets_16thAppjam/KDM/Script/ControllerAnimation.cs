using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandState { Idle, Grab, LargeGrab}

public class ControllerAnimation : Controller {

    #region Variables

    public bool isLock = false;

    #endregion
    
    #region Methods

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
