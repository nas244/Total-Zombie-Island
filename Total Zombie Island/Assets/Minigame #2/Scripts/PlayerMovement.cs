using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // init some vars editable in Unity
    [SerializeField] private float runSpeed;

    // init some vars NOT editable in Unity
    private Animator animator;
    private Rigidbody rb;
    private float horizontalInput;
    private bool isAttacking = false;
    private bool detectingHit = false;
    private weaponTypes weaponType;

    // enumerator for weapon types
    enum weaponTypes {
        MELEE,
        SHOTGUN
    }

    // Start is called before the first frame update
    void Start()
    {
        // grab the RigidBody
        gameObject.TryGetComponent<Rigidbody>(out rb);

        // grab the animator
        gameObject.TryGetComponent<Animator>(out animator);
        
    }

    // Update is called once per frame
    void Update()
    {
        // get input from the player
        horizontalInput = Input.GetAxis("Horizontal");

        // check if the player is attacking
        if (Input.GetMouseButton(0))
        {
            isAttacking = true;
            animator.SetInteger("WeaponType_int", 12);
            animator.SetInteger("MeleeType_int", 2);
        }
    }

    // FixedUpdate is called once every fixedDeltaTime
    private void FixedUpdate()
    {

        // Check if the player is moving
        if (!isAttacking && horizontalInput != 0.0f)
        {
            // check if the player is trying to move left
            if (horizontalInput < 0)
            {
                // rotate the player to look left
                gameObject.transform.eulerAngles = new Vector3(
                    gameObject.transform.eulerAngles.x,
                    0,
                    gameObject.transform.eulerAngles.z
                );
            }

            // else the player must be moving right
            else
            {
                // rotate the player to look right
                gameObject.transform.eulerAngles = new Vector3(
                    gameObject.transform.eulerAngles.x,
                    180,
                    gameObject.transform.eulerAngles.z
                );
            }

            // set animator to running
            animator.SetFloat("Speed_f", 1.0f);
            animator.SetInteger("WeaponType_int", 0);
            animator.SetInteger("MeleeType_int", 0);

            // add force to the player to move forward
            Vector3 moveDirection = transform.forward * runSpeed;
            rb.AddForce(moveDirection);
        }

        // else the player is not moving stationary
        else
        {
            animator.SetFloat("Speed_f", 0.0f);
        }
    }

    // function called at the end of the attack animation
    void AttackEndEvent()
    {
        // stop the attack animation
        isAttacking = false;
        detectingHit = false;
        animator.SetInteger("WeaponType_int", 0);
        animator.SetInteger("MeleeType_int", 0);
    }

    // function called when the player's attack could hit something
    void AttackPeakEvent()
    {
        // check if this is the first time this has been called
        if (!detectingHit)
        {
            detectingHit = true;

            // use raycasting to detect a hit
            RaycastHit hit;
            if (Physics.Raycast (transform.position, transform.forward, out hit))
            {
                // check if the object hit is a zombie
                GameObject zombieObj = hit.collider.gameObject;
                if (zombieObj.CompareTag("zombie"))
                {
                    switch (weaponType)
                    {
                        case weaponTypes.MELEE:
                            // check to see if it was within distance
                            if (hit.distance < 3)
                            {
                                // call Die on the zombie
                                Debug.Log(zombieObj.name);
                                zombieObj.GetComponent<Zombie>().Die();
                            }
                            break;
                    }
                }
            }
        }
    }
}
