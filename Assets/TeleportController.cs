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

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool pressed))
        {
            if (touched && pressed && !isPressed)
            {
                isPressed = true;
                OnPressedStart();
            }
            else if (!pressed && isPressed)
            {
                isPressed = false;
                OnPressedEnd();
            }
        }

        if (isPressed && Physics.Raycast(laserPointer.transform.position, laserPointer.transform.forward,
                out RaycastHit hit, 100f, layerMask))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                teleportRing.SetActive(true);
                teleportRing.transform.position = hit.point;
                teleportRing.transform.rotation = hit.collider.transform.rotation;
            }
            else
            {
                teleportRing.SetActive(false);
            }
        }
        else
        {
            teleportRing.SetActive(false);
        }
    }

    void OnPressedStart()
    {

    }

    void OnPressedEnd()
    {

        if (Physics.Raycast(laserPointer.transform.position, laserPointer.transform.forward,
            out RaycastHit hit, 100f, layerMask))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                Vector3 difference = rigTransform.position - headTransform.position;
                difference.y = 0f;
                rigTransform.position = hit.point + difference;
            }
        }
    }

    void OnDisable()
    {

        laserPointer.SetActive(false);
        teleportRing.SetActive(false);
    }

    void OnEnable()
    {
        laserPointer.SetActive(true);
        teleportRing.SetActive(true);
    }
}
