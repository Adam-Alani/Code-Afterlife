using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 10f; // must be > 10f
    public Vector3 offset;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    
    // update in last, useful to let the player move first
    void LateUpdate()
    {
       
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
