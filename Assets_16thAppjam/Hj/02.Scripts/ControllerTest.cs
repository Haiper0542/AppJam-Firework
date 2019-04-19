using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{

	public bool isRight = true;
	
	private SteamVR_TrackedObject trackedObj;
	
	SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)trackedObj.index); }}

	public MainController mainController;
	
	void Awake ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void Update () {
		if (Controller.GetHairTriggerDown())
		{
			if (isRight)
			{
				mainController.RTriggerDown();
			}
			else
			{
				mainController.LTriggerDown();
			}
		}

		if (Controller.GetHairTriggerUp())
		{
			if (isRight)
			{
				mainController.RTriggerUp();
			}
			else
			{
				mainController.LTriggerUp();
			}
		}
	}
}
