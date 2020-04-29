﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
 
    void Update()
    {
        transform.LookAt(target);
        transform.Translate(Vector3.right * (Time.deltaTime* 10));
    }
}
