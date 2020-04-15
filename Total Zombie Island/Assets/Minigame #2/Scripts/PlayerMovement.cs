using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // init some vars editable in Unity
    [SerializeField] private float runSpeed;
    [SerializeField] private GameObject weaponSlot;

    // init some vars NOT editable in Unity
    private Animator animator;
    private Rigidbody rb;
    private float horizontalInput;
    private bool isAttacking = false;
    private bool attackCalled = false;
    private bool detectingHit = false;
    private weaponTypes weaponType = weaponTypes.NONE;
    private EquipingWeapon equip;

    // enumerator for weapon types
    enum weaponTypes {
        NONE,
        MELEE,
        SHOTGUN,
        MINIGUN,
        ASSAULT,
        PISTOL,
    }

    // Start is called before the first frame update
    void Start()
    {
        // grab the RigidBody
        gameObject.TryGetComponent<Rigidbody>(out rb);

        // grab the animator
        gameObject.TryGetComponent<Animator>(out animator);

        // grab the equip script
        equip = weaponSlot.GetComponent<EquipingWeapon>();

        // setup the animator
        animator.SetInteger("MeleeType_int", -1);
        animator.SetBool("Shoot_b", false);
    }

    // Update is called once per frame
    void Update()
    {
        // get input from the player
        horizontalInput = Input.GetAxis("Horizontal");

        // check if the player is attacking
        if (Input.GetMouseButton(0) && weaponType != weaponTypes.NONE) attackCalled = true;
    }

    // FixedUpdate is called once every fixedDeltaTime
    private void FixedUpdate()
    {
        // check if the player is trying to attack
        if (attackCalled)
        {
            Attack();
        }

        // the player is attacking
        else if (!isAttacking && horizontalInput != 0)
        {
            Move();
        }

        // the player is doing nothing
        else
        {
            animator.SetFloat("Speed_f", 0.0f);
        }
    }

    void Move()
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

        // add force to the player to move forward
        Vector3 moveDirection = transform.forward * runSpeed;
        rb.AddForce(moveDirection);
    }

    void Attack()
    {
        // setup flags
        attackCalled = false;

        // check if already attacking or if a weapon is equipped
        if (isAttacking) return;

        // check if the player is looking left
        if (weaponType != weaponTypes.MELEE && gameObject.transform.eulerAngles.y == 0.0f)
        {
            // rotate the player to look left
            gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                50,
                gameObject.transform.eulerAngles.z
            );
        }

        // else if the player is looking right
        else if (weaponType != weaponTypes.MELEE && gameObject.transform.eulerAngles.y == 180.0f)
        {
            // rotate the player to look right
            gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                235,
                gameObject.transform.eulerAngles.z
            );
        }

        // play appropriate attack animation
        if (weaponType == weaponTypes.MELEE)
        {
            animator.SetInteger("MeleeType_int", 2);
        }
        else
        {
            animator.SetBool("Shoot_b", true);
        }

        // setup flags
        isAttacking = true;
    }

    // function called at the end of the attack animation
    void AttackEndEvent()
    {
        // stop the attack animation
        isAttacking = false;
        detectingHit = false;
        animator.SetInteger("MeleeType_int", -1);
        animator.SetBool("Shoot_b", false);
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

    public void EquipWeapon(string type)
    {
        switch (type)
        {
            case "Shotgun":
                // equip the player's shotgun
                equip.ShotgunWeapon();
                animator.SetInteger("WeaponType_int", 4);
                weaponType = weaponTypes.SHOTGUN;
                break;
            case "Katana":
                // equip the player's katana
                equip.HandWeapon();
                animator.SetInteger("WeaponType_int", 12);
                weaponType = weaponTypes.MELEE;
                break;
            case "Minigun":
                // equip the player's minigun
                equip.MinigunWeapon();
                animator.SetInteger("WeaponType_int", 9);
                weaponType = weaponTypes.MINIGUN;
                break;
            case "Assault":
                // equip the player's assault gun
                equip.ARWeapon();
                animator.SetInteger("WeaponType_int", 2);
                weaponType = weaponTypes.ASSAULT;
                break;
        }

        Debug.Log("Equipped \"" + type + "\".");
    }
}
