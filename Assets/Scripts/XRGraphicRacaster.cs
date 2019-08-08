using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class XRGraphicRacaster : MonoBehaviour
{
    public GameObject laserPointer;
    public XRNode nodeType;
    private LayerMask layerMask;

    public List<Behaviour> disableOnUIBehaviour = new List<Behaviour>();

    public GameObject pointerEnterObject;
    public GameObject pointerDownObject;

    public bool isPressed;

    private void Start()
    {
        layerMask = LayerMask.GetMask("UI");
    }

    // Update is called once per frame
    void Update()
    {
        HandleEnterAndExit();
        HandleDownUpAndClick();
    }

    void HandleEnterAndExit()
    {
        if (Physics.Raycast(laserPointer.transform.position, laserPointer.transform.forward, out RaycastHit hit, 100, layerMask))
        {
            GraphicRaycaster graphicRaycaster = hit.collider.GetComponentInParent<GraphicRaycaster>();
            if (graphicRaycaster != null)
            {
                laserPointer.SetActive(true);
                SetObjectsActive(false);

                Vector3 screenPoint = Camera.main.WorldToScreenPoint(hit.point);
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = screenPoint;
                List<RaycastResult> list = new List<RaycastResult>();
                graphicRaycaster.Raycast(eventData, list);
                if (list.Count > 0)
                {
                    foreach (var target in list)
                    {
                        if (pointerEnterObject == target.gameObject)
                        {
                            break;
                        }
                        var pointer = new PointerEventData(EventSystem.current);
                        if (ExecuteEvents.Execute(target.gameObject, pointer, ExecuteEvents.pointerEnterHandler))
                        {
                            ClearFocus();
                            pointerEnterObject = target.gameObject;
                            break;
                        }
                    }
                    return;
                }
            }

        }

        ClearFocus();
        laserPointer.SetActive(false);
        SetObjectsActive(true);
    }

    void HandleDownUpAndClick()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(nodeType);

        bool touched = false;
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 touchAxis))
        {
            if (Mathf.Abs(touchAxis.x) < .5f &&
               touchAxis.y > .5f)
            {
                touched = true;
            }
        }

        //laserPointer.SetActive(touched || isPressed);

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool pressed))
        {
            if (touched && pressed && !isPressed)
            {
                isPressed = true;
                OnPressStart();
            }
            else if (pressed == false && isPressed)
            {
                isPressed = false;
                OnPressEnd();
            }
        }
    }
    void OnPressStart()
    {
        if (pointerEnterObject)
        {
            pointerDownObject = pointerEnterObject;
            var pointer = new PointerEventData(EventSystem.current);
            if (ExecuteEvents.Execute(pointerDownObject, pointer, ExecuteEvents.pointerDownHandler))
            {
            }
        }
    }

    void OnPressEnd()
    {
        var pointer = new PointerEventData(EventSystem.current);
        if (ExecuteEvents.Execute(pointerDownObject, pointer, ExecuteEvents.pointerUpHandler))
        {
        }
        if (pointerDownObject == pointerEnterObject)
        {
            if (ExecuteEvents.Execute(pointerDownObject, pointer, ExecuteEvents.pointerClickHandler))
            {

            }
        }
        pointerDownObject = null;
    }

    void ClearFocus()
    {
        if (!pointerEnterObject)
        {
            return;
        }
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(pointerEnterObject, pointer, ExecuteEvents.pointerExitHandler);
        pointerEnterObject = null;
    }

    void SetObjectsActive(bool active)
    {
        foreach (var behaviour in disableOnUIBehaviour)
        {
            behaviour.enabled = active;
        }
    }
}