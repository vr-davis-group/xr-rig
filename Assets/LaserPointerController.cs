using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LaserPointerController : MonoBehaviour
{
    public XRNode nodeType;
    public GameObject laserPointer;

    private bool isPressed;

    void OnDisable()
    {
        laserPointer.SetActive(false);
    }

    void OnEnable()
    {
        laserPointer.SetActive(true);
    }

    void Update()
    {
        // Set input device
        InputDevice device = InputDevices.GetDeviceAtXRNode(nodeType);

        // Is top of touch pad touched
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

}
