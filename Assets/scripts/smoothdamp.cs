using UnityEngine;
using System.Collections;

public class SmoothDamp : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

