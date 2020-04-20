using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // define some vars editable in Unity
    [SerializeField] public int health;
    [SerializeField] private int lookRadius;
    [SerializeField] private int moveSpeed;
    [SerializeField] private float attackDelay;
    [SerializeField] private int attackDistance;
    [SerializeField] public int attackDamage;
    [SerializeField] private GameObject bloodSplatter;
    [SerializeField] private GameObject bloodSpot;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] hurtSounds;

    // define some vars NOT editable in Unity
    private Animator anim;
    private Rigidbody rb;
    private GameObject target;
    private bool targetDetected;
    private float detectionTime;
    [System.NonSerialized] public GameObject parentSpawner;

    // Start is called before the first frame update
    void Start()
    {
        // try to grab the zombie's animator component
        gameObject.TryGetComponent<Animator>(out anim);
        if (!anim) Debug.LogError("Couldn't find animator for zombie!");

        // try to grab the Rigidbody component
        gameObject.TryGetComponent<Rigidbody>(out rb);
        if (!rb) Debug.LogError("Couldn't find rigidbody for zombie!");

        // try to find the target player
        target = GameObject.FindGameObjectWithTag("Player");
        if (!target) Debug.LogError("Could not find tagged \"Player\" object!");
    }

    // Update is called once per frame
    void Update()
    {
        // if the zombie isn't dead, chase the player
        if (health > 0) Move();

        // check if the zombie has noclipped through the map
        if (health > 0 && transform.position.y < 0)
        {
            Destroy(this.gameObject);
            Debug.Log("Zombie destroyed for leaving the map.");
        }
    }

    // makes the zombie chase the player
    private void Move()
    {
        Vector3 directionToTarget;
        float distance = Vector3.Distance(target.transform.position, transform.position);

        // Gives enemy a range that will cause it to move once the player enters it
        if (distance <= lookRadius)
        {
            // if the enemy is within attackDistance range, attack the player
            if (distance < attackDistance)
            {
                // check if we've detected the player already and it's been longer than attackDelay seconds
                if (targetDetected && (Time.time - detectionTime) > attackDelay)
                {
                    // reset the detectionTime
                    detectionTime = Time.time;

                    // if the player isn't dead, attack
                    if (target.GetComponent<PlayerMovement>().health > 0)
                    {
                        Attack();
                    }

                    // else play the feeding animation
                    else
                    {
                        anim.Play("Zombie_Eating");
                    }
                }

                // else if we have just discovered the player, wait to attack
                else if (!targetDetected)
                {
                    targetDetected = true;
                    detectionTime = Time.time;
                }
            }

            // otherwise, move closer
            else
            {
                // reset targetDetected
                targetDetected = false;

                // Rotate the enemy to always face the player. Locks every axis except Y so that it only rotates on Y axis. Change this for 3D sections
                Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                transform.LookAt(targetPosition);

                // Move enemy towards player
                directionToTarget = (target.transform.position - transform.position).normalized;
                rb.velocity = new Vector2(directionToTarget.z * moveSpeed, 0);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, target.transform.position.z), moveSpeed * Time.deltaTime);
            }
        }
    }

    // attacks the player
    void Attack()
    {
        // grab the PlayerMovement component from the player
        PlayerMovement player = target.GetComponent<PlayerMovement>();

        // call Hurt on the player for the specified amount of damage
        player.Hurt(attackDamage);
    }

    // function called when a zombie gets hit
    public void Hurt(int damage)
    {
        // subtract from the zombie's health
        health -= damage;

        // instantiate blood splatter object on zombie
        Destroy(Instantiate(bloodSplatter, bloodSpot.transform), 1.0f);

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
        // play a random death sound
        float chance = Random.Range(0.0f, 10.0f);
        if (chance < 2.5)
        {
            audioSource.PlayOneShot(hurtSounds[1]);
        }
        else if (chance < 5.0)
        {
            audioSource.PlayOneShot(hurtSounds[2]);
        }
        else
        {
            audioSource.PlayOneShot(hurtSounds[0]);
        }
        

        // disable the root components
        anim.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;

        // unlock the X and Y axis'
        rb.constraints = RigidbodyConstraints.None;

        // enable the ragdoll
        RagdollEnable();

        // if this zombie has been spawned, update parentSpawner
        if (parentSpawner) parentSpawner.GetComponent<Spawning>().UpdateCount();
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
