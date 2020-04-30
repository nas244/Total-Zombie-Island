using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    float speed;

    void Update()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.right * (Time.deltaTime* speed));
    }
}
