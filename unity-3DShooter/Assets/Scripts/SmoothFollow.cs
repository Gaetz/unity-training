using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmoothFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 velocity = Vector3.one;
    public Vector3 defaultDistance;
    public float distance;

    
    void LateUpdate()
    {
        if (!target) return;
        
        Vector3 toPosition = target.position + (target.rotation * defaultDistance);
        Vector3 currentPosition = Vector3.SmoothDamp(transform.position, toPosition, ref velocity, distance);
        transform.position = currentPosition;

        transform.LookAt(target);
    }
}