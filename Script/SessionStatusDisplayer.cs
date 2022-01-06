using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using ManoMotion;

public class SessionStatusDisplayer : MonoBehaviour
{
    private TextMeshProUGUI sessionStatusText;

    // Start is called before the first frame update
    void Start()
    {
        //Subscribe  to the Event that fires when the session has changed status
        ARSession.stateChanged += HandleStateChanged;
        sessionStatusText = this.GetComponent<TextMeshProUGUI>( );
    }

    /// <summary>
    /// Handles the situation when the status of the Session has changed. In this case  update the text information according to what the status changes to 
    /// </summary>
    /// <param name="statEventArguments"></param>
    private void HandleStateChanged(ARSessionStateChangedEventArgs statEventArguments)
    {
        switch (statEventArguments.state)
        {
            case ARSessionState.None:
                sessionStatusText.text = "Session Status: Unknown";
                break;
            case ARSessionState.Unsupported:
                sessionStatusText.text = "Session Status: ARFoundation not supported";
                break;
            case ARSessionState.CheckingAvailability:
                sessionStatusText.text = "Checking availability";
                break;
            case ARSessionState.NeedsInstall:
                sessionStatusText.text = "Needs Install";
                break;
            case ARSessionState.Installing:
                sessionStatusText.text = "Installing";
                break;
            case ARSessionState.Ready:
                sessionStatusText.text = "Ready";
                break;
            case ARSessionState.SessionInitializing:
                sessionStatusText.text = "Poor SLAM Quality";
                break;
            case ARSessionState.SessionTracking:
                sessionStatusText.text = "Tracking quality is Good";
                break;
            default:
                sessionStatusText.text = "Session Status: Unknown";
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
