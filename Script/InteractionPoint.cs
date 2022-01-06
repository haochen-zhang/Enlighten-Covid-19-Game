using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class InteractionPoint : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (ARSession.state == ARSessionState.SessionTracking)
        {
            FollowPalmCenter();
        }
    }

    private void FollowPalmCenter( )
    {
        HandInfo currentlyDetectedHand = ManomotionManager.Instance.Hand_infos[0].hand_info;
        ManoClass currentlyDetectedManoClass = currentlyDetectedHand.gesture_info.mano_class;
        Vector3 palmCenterPosition = currentlyDetectedHand.tracking_info.palm_center;
        

        if (currentlyDetectedManoClass == ManoClass.POINTER_GESTURE)
        {
            this.GetComponent<Renderer>().enabled = true;
            this.transform.position = ManoUtils.Instance.CalculateNewPosition(palmCenterPosition, currentlyDetectedHand.tracking_info.depth_estimation);
        }
        else
        {
            this.GetComponent<Renderer>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with something");
        if (other.gameObject.tag == "Player")
        {
            Handheld.Vibrate();
            this.GetComponent<Renderer>().material.color = Color.red;
            Destroy(other.gameObject);
            StartCoroutine(TimeDelay());
            this.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    IEnumerator TimeDelay()
    {
        print(Time.time);
        yield return new WaitForSeconds(500);
        print(Time.time);
    }
}
