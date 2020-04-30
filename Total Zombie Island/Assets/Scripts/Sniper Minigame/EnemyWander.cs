using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    public float speed = 3;
    private float waitTime;
    public float startWaitTime = 3;
    public float lookRadius = 20f;

    GameObject emptyGO;
    Transform moveSpot;

    Transform origin;
    public GameObject deathEffect;
    public GameObject hitEffect;
    public ParticleSystem ptc;

    Rigidbody rb;
    Animator animator;

    GameObject childObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        origin = GetComponent<Transform>();

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        emptyGO = new GameObject();
        moveSpot = emptyGO.transform;

        CreateBoxCollider("Head_jnt", new Vector3(-0.4f, 0f, 0f), new Vector3(0.8f, 0.7f, 0.7f));
        CreateBoxCollider("Chest_jnt", new Vector3(0.25f, 0.015f, 0.0045f), new Vector3(1.1f, 0.9f, 0.65f));
        CreateBoxCollider("Arm_Left_jnt", new Vector3(0.5f, 0f, 0f), new Vector3(1.25f, 0.25f, 0.3f));
        CreateBoxCollider("Arm_Right_jnt", new Vector3(-0.5f, 0f, 0f), new Vector3(1.25f, 0.25f, 0.3f));
        CreateBoxCollider("UpperLeg_Left_jnt", new Vector3(-0.4f, 0f, 0f), new Vector3(1f, 0.25f, 0.35f));
        CreateBoxCollider("UpperLeg_Right_jnt", new Vector3(0.4f, 0f, 0f), new Vector3(1f, 0.25f, 0.35f));

        waitTime = startWaitTime;

        moveSpot.position = new Vector3(origin.transform.position.x + Random.Range(-lookRadius, lookRadius), transform.position.y, origin.transform.position.z + Random.Range(-lookRadius, lookRadius));
    }

    private void Update()
    {
        //animator.SetBool("isMoving", true);

        Vector3 targetPosition = new Vector3(moveSpot.transform.position.x, transform.position.y, moveSpot.transform.position.z);
        transform.LookAt(moveSpot.position);

        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveSpot.position) < 1f)
        {
            FreezePosition();
            //animator.SetBool("isMoving", false);
            if (waitTime <= 0)
            {
                UnfreezePosition();
                moveSpot.position = new Vector3(origin.transform.position.x + Random.Range(-lookRadius, lookRadius), transform.position.y, origin.transform.position.z + Random.Range(-lookRadius, lookRadius));
                waitTime = startWaitTime;
            }

            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void CreateBoxCollider(string name, Vector3 center, Vector3 size)
    {
        childObject = GameObject.Find(name);
        if (name == "Head_jnt")
        {
            childObject.transform.tag = "Head";
        }
        else
            childObject.transform.tag = "Enemy";

        BoxCollider collider = childObject.AddComponent<BoxCollider>();
        collider.center = center;
        collider.size = size;
    }

    void OnRaycastReceived()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        GameObject heffect = Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);

        heffect.transform.eulerAngles = new Vector3(0, 90, 0);
        ptc = ptc.GetComponent<ParticleSystem>();
        ptc.Play();

        Destroy(effect, 1f);
        Destroy(heffect, 1f);
        Destroy(emptyGO);
        Destroy(moveSpot);
        gameObject.SetActive(false);
    }

    void FreezePosition()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    void UnfreezePosition()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Building":
                moveSpot.position = new Vector3(origin.transform.position.x + Random.Range(-lookRadius, lookRadius), transform.position.y, origin.transform.position.z + Random.Range(-lookRadius, lookRadius));
                break;

            case "Obstacle":
                moveSpot.position = new Vector3(origin.transform.position.x + Random.Range(-lookRadius, lookRadius), transform.position.y, origin.transform.position.z + Random.Range(-lookRadius, lookRadius));
                break;
        }
    }
}
