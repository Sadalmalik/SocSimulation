using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float yPos = 30;
    public float speed = 5;
    public float radius = 50;

    void Update()
    {
        float angle = Time.time * speed * Mathf.Deg2Rad;

        transform.position = new Vector3(
            radius * Mathf.Cos(angle),
            yPos,
            radius * Mathf.Sin(angle));

        Vector3 point = target != null ? target.position : Vector3.zero;

        transform.LookAt(point);
    }
}
