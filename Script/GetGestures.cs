using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;


public class GetGestures : MonoBehaviour
{
	//definition
	public AudioClip clip;
	public GameObject PalmModel;
	public GameObject[] Effect;                                                     //for effectObject
	public GameObject[] PrefabItem;                                           //for portalModel
	AudioSource source;
	GameObject prefabEffect;
	GameObject toShow;
	Vector3 newPos;
	int i = 0;
	public Text PalmPositionInfo;
	public Text GestureStatus;
	public Text HandStatus;
	public Text EffectName;
	string HandStatusMessage;
	string GestureStatusMessage;
	int handsSupportedByLicence = 1;
	bool ShowPalmObject = true;
	bool once = true;
	bool isSessionQualityOK;


	void Start()
	{
		ARSession.stateChanged += HandleStateChanged;
	}

	/// <summary>
	/// Handles the situation when the status of the Session has changed. IIn this  case keep track if the  session is in good  quality and tracking
	/// </summary>
	/// <param name="stateEventArguments"></param>
	private void HandleStateChanged(ARSessionStateChangedEventArgs stateEventArguments)
	{
		isSessionQualityOK = stateEventArguments.state == ARSessionState.SessionTracking;
	}

	void Next()
	{
		i++;
		if (i == Effect.Length)
			i = 0;
	}

	void ShowEffect()
	{
		RemoveEffect();
		prefabEffect = Instantiate(Effect[i]);
		prefabEffect.transform.position = newPos;
		prefabEffect.SetActive(true);
		EffectName.text = prefabEffect.name.Replace("Clone", "");
	}

	void ShowPortal()
    {
		toShow = Instantiate(PrefabItem[i]);
		toShow.SetActive(true);
	}

	void Update()
	{
		// update Model and UI
		if (isSessionQualityOK)
		{
			DisplayDetectedState();
			DetectUserGesture();
		}
		// update Cordinate
		for (int handIndex = 0; handIndex < handsSupportedByLicence; handIndex++)
		{
			ShowPalmCenter(ManomotionManager.Instance.Hand_infos[handIndex].hand_info.tracking_info, handIndex);
		}
	}

	void ShowPalmCenter(TrackingInfo tracking_info, int index)
	{
		if (ShowPalmObject)
		{
			Vector3 palmCenter = tracking_info.palm_center;
			float depth = tracking_info.depth_estimation;
			newPos = ManoUtils.Instance.CalculateNewPosition(palmCenter, depth);
			PalmModel.transform.position = newPos;
			if (prefabEffect != null)
				prefabEffect.transform.position = newPos;
			PalmPositionInfo.text = newPos.ToString();
		}
	}

	void DisplayDetectedState()
	{
		HandInfo handInformation = ManomotionManager.Instance.Hand_infos[0].hand_info;
		GestureInfo gestureInormation = handInformation.gesture_info;
		GestureStatusMessage = "The gesture is in " + gestureInormation.state.ToString() + " state.";
		GestureStatus.text = GestureStatusMessage;
	}

	/// <summary>
	/// Overflow Check of the void model prefab in Unity
	/// </summary>
	void RemoveEffect()
	{
		if (prefabEffect != null)
			Destroy(prefabEffect);
	}

	/// <summary>
	/// Main Condition Estimate Function According to The User's Gesture Input
	/// </summary>
	void DetectUserGesture()
	{
		HandInfo handInformation = ManomotionManager.Instance.Hand_infos[0].hand_info;
		GestureInfo gestureInormation = handInformation.gesture_info;
		ManoGestureTrigger currentDetectedTriggerGesture = gestureInormation.mano_gesture_trigger;

		// estimate the gesture, and put AR action accordingly
		// click gesture
		 if (currentDetectedTriggerGesture == ManoGestureTrigger.CLICK)
		{
			ShowPortal();
			GameObject newItem = Instantiate(toShow);
			Vector3 positionToMove = Camera.main.transform.position + (Camera.main.transform.forward * 2);
			newItem.transform.position = positionToMove;
			Handheld.Vibrate();
		}
		 // release gesture
		else if (currentDetectedTriggerGesture == ManoGestureTrigger.RELEASE_GESTURE)
		{
			HandStatusMessage = "Opened Hand";
			if (once)
			{
				source = gameObject.AddComponent<AudioSource>();
				source.clip = clip;
				source.PlayOneShot(clip);
				Next();
				ShowEffect();
				once = false;
			}
		}
		 // grab gesture
		else if (currentDetectedTriggerGesture == ManoGestureTrigger.GRAB_GESTURE)
		{
			HandStatusMessage = "Grab Hand";
			if (!once)
			{
				RemoveEffect();
				once = true;
			}
		}
		
		HandStatus.text = HandStatusMessage;
	}
}



