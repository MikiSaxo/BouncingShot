using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed;
    public Vector3 offset;
    [SerializeField] bool isSmooth;

    private void Update()
    {
        if (isSmooth)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothPosition;
        }
        else
        {
            transform.position = target.position;
            //transform.position = Vector3.MoveTowards(transform.position, target.position, 3000);
        }
    }
}
