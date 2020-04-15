﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // define some vars NOT editable in Unity
    private Animator anim;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // try to grab the zombie's animator component
        gameObject.TryGetComponent<Animator>(out anim);
        if (!anim) Debug.LogError("Couldn't find animator for zombie!");

        // try to grab the Rigidbody component
        gameObject.TryGetComponent<Rigidbody>(out rb);
        if (!rb) Debug.LogError("Couldn't find rigidbody for zombie!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // fucntion called when the zombie dies
    public void Die()
    {
        Debug.Log("Die has been called!");

        // disable the root components
        anim.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;

        // enable the ragdoll
        RagdollEnable();

        // unlock the X and Y axis'
        rb.constraints = RigidbodyConstraints.None;

    }

    public void RagdollEnable()
    {
        // enable all boxColliders
        foreach (BoxCollider box in gameObject.GetComponentsInChildren<BoxCollider>())
        {
            box.enabled = true;
            Debug.Log("BoxCollider enabled.");
        }

        // enable all sphereColliders
        foreach (SphereCollider sphere in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            sphere.enabled = true;
        }

        // enable all capsuleColliders
        foreach (CapsuleCollider capsule in gameObject.GetComponentsInChildren<CapsuleCollider>())
        {
            capsule.enabled = true;
        }
    }
}
