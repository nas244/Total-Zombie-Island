using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // define some vars editable in Unity
    [SerializeField] private int health;
    [SerializeField] private int knockback;
    [SerializeField] private GameObject bloodSplatter;
    [SerializeField] private GameObject bloodSpot;

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

    // function called when a zombie gets hit
    public void Hurt(int damage)
    {
        Debug.Log("Zombie \"" + this.name + "\" has been hurt!");

        // subtract from the zombie's health
        health -= damage;

        // instantiate blood splatter object on zombie
        Destroy(Instantiate(bloodSplatter, bloodSpot.transform), 1.0f);

        // knock the zombie back a bit
        //Vector3 moveDirection = transform.forward * -knockback * 100;
        //rb.AddForce(moveDirection);

        // check if the zombie is dead
        if (health <= 0)
        {
            // call Die on this zombie
            Die();
        }
    }

    // fucntion called when the zombie dies
    void Die()
    {
        Debug.Log("Zombie \"" + this.name + "\" is dead!");

        // disable the root components
        anim.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;

        // unlock the X and Y axis'
        rb.constraints = RigidbodyConstraints.None;

        // enable the ragdoll
        RagdollEnable();
    }

    void RagdollEnable()
    {
        // enable all boxColliders
        foreach (BoxCollider box in gameObject.GetComponentsInChildren<BoxCollider>())
        {
            box.enabled = true;
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
