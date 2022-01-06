using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;


public class Clicker : MonoBehaviour
{
    public AudioClip clip;
    public GameObject PalmModel;
    public GameObject[] Effect;
    AudioSource source;
    GameObject prefabEffect;
    Vector3 newPos;
    public Text PalmPositionInfo;
    public Text GestureStatus;
    public Text HandStatus;
    public Text EffectName;
    bool isSessionQualityOK;
    // Start is called before the first frame update
    void Start()
    {
        ARSession.stateChanged += HandleStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSessionQualityOK)
        {
            SpwanCubeWithClickTriggerGesture( );
        }
    }
    /// <summary>
    /// Handles the situation when the status of the Session has changed. IIn this  case keep track if the  session is in good  quality and tracking
    /// </summary>
    /// <param name="stateEventArguments"></param>
    private void HandleStateChanged(ARSessionStateChangedEventArgs stateEventArguments)
    {
        isSessionQualityOK = stateEventArguments.state == ARSessionState.SessionTracking;
    }

    //TODO
    //Replace  this with a prefab of items you feel like spawning.
    public GameObject itemPrefab;

    private void SpwanCubeWithClickTriggerGesture( )
    {
        HandInfo handInformation = ManomotionManager.Instance.Hand_infos[0].hand_info;
        GestureInfo gestureInormation = handInformation.gesture_info;
        ManoGestureTrigger currentDetectedTriggerGesture = gestureInormation.mano_gesture_trigger;

        if (currentDetectedTriggerGesture == ManoGestureTrigger.CLICK)
        {
            GameObject newItem = Instantiate(itemPrefab);
            Vector3 positionToMove = Camera.main.transform.position + (Camera.main.transform.forward * 2);
            newItem.transform.position = positionToMove;
            Handheld.Vibrate();
        }
    }

}
