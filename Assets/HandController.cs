using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandController : MonoBehaviour
{
    public XRNode nodeType;
    public GameObject model;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(nodeType);
        if (!device.isValid)
        {
            model.SetActive(false);
        }
        else
        {
            model.SetActive(true);
        }

    }
}
