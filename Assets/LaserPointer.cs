using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    public LayerMask layerMask;

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 100f, layerMask))
        {
            transform.localScale = new Vector3(1, 1, hit.distance);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 100f);
        }
    }
}
