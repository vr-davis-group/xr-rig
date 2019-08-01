using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TeleportController : MonoBehaviour
{
    public XRNode nodeType;
    public bool isPressed;
    public GameObject laserPointer;
    public GameObject teleportRing;
    public LayerMask teleportMask;
    public Transform rigTransform;
    public Transform headTransform;
    public LayerMask layerMask;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(nodeType);
        bool touched = false;

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 touchAxis))
        {
            if (Mathf.Abs(touchAxis.x) < .5f && touchAxis.y > .5f)
            {
                touched = true;
            }
        }

        laserPointer.SetActive(touched || isPressed);
    }

    void OnPressedStart()
    {

    }

    void OnPressedEnd()
    {


    }

    void OnDisable()
    {


    }
}
