using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaceOnPalm : MonoBehaviour
{
    bool showButterfly = true;

    private void Start()
    {
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ARSession.state == ARSessionState.SessionTracking)
        {
            FollowPalmCenter();
        }
    }

    private void FollowPalmCenter()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        HandInfo currentlyDetectedHand = ManomotionManager.Instance.Hand_infos[0].hand_info;
        ManoGestureContinuous currentlyDetectedContinousGesture = currentlyDetectedHand.gesture_info.mano_gesture_continuous;

        Vector3 pinchPos = currentlyDetectedHand.tracking_info.palm_center;

        if (currentlyDetectedContinousGesture == ManoGestureContinuous.OPEN_HAND_GESTURE)
        {
            Vector3 newPos = ManoUtils.Instance.CalculateNewPosition(pinchPos, currentlyDetectedHand.tracking_info.depth_estimation);
            if (!showButterfly)
            {
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = true;
                }
                transform.position = newPos;
                showButterfly = true;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
            }
        }
        else
        {
            if (showButterfly)
            {
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = false;
                }
                showButterfly = false;
            }
        }
    }
}
