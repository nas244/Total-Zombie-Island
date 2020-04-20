using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // init some vars editable in Unity
    [SerializeField] private float runSpeed;
    [SerializeField] private int startingHealth;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private GameObject shotLocation;
    [SerializeField] private GameObject meleePoint;
    [SerializeField] private GameObject dirtSplatter;
    [SerializeField] private GameObject ammoUI;
    [SerializeField] private GameObject healthUI;
    [SerializeField] private GameObject weaponUI;
    [SerializeField] private GameObject skin;

    // init some vars NOT editable in Unity
    private Animator animator;
    private Rigidbody rb;
    private float horizontalInput;
    private bool isAttacking = false;
    private bool attackCalled = false;
    private bool detectingHit = false;
    private weaponTypes weaponType = weaponTypes.NONE;
    private EquipingWeapon equip;
    private int ammo = 0;
    [System.NonSerialized] public int health;
    private bool isHurt;
    private float hurtTime;
    private GameManager gameManager;

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

        // init health
        health = startingHealth;

        // grab the game manager
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

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
        if (Input.GetMouseButton(0) && weaponType != weaponTypes.NONE && ammo > 0) attackCalled = true;

        // update the UI
        UpdateUI();

        // if the player is hurt, flash the player red
        if (isHurt)
        {
            SkinnedMeshRenderer skinMesh = skin.GetComponent<SkinnedMeshRenderer>();
            float t = (Time.time - hurtTime);
            skinMesh.material.color = Color.Lerp(Color.white, Color.red, t);
            skinMesh.material.color = Color.Lerp(Color.red, Color.white, t);
            if (skinMesh.material.color == Color.white)
            {
                isHurt = false;
            }
        }

        // check if the player is dead
        if (health <= 0)
        {
            // play the death animation
            animator.SetBool("Death_b", true);

            // call game over
            gameManager.GameOver(false);
        }
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

    void UpdateUI()
    {
        // update the ammo UI
        if (ammo > 0) ammoUI.GetComponent<TextMesh>().text = ammo.ToString();
        else ammoUI.GetComponent<TextMesh>().text = "NO AMMO";

        // update the health UI
        healthUI.GetComponent<TextMesh>().text = health.ToString() + "/" + startingHealth.ToString();

        // update the weapon text
        string newText;
        switch (weaponType)
        {
            case weaponTypes.NONE:
                newText = "Fisticuffs";
                break;
            case weaponTypes.ASSAULT:
                newText = "Assault Rifle";
                break;
            case weaponTypes.MINIGUN:
                newText = "Minigun";
                break;
            case weaponTypes.SHOTGUN:
                newText = "Shotgun";
                break;
            case weaponTypes.PISTOL:
                newText = "Pistol";
                break;
            case weaponTypes.MELEE:
                newText = "Katana";
                break;
            default:
                newText = "Fisticuffs";
                break;
        }
        weaponUI.GetComponent<TextMesh>().text = newText;
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

        // check if the player has any ammo left
        if (ammo <= 0) return;

        // check if the player is looking left
        if (weaponType != weaponTypes.MELEE && gameObject.transform.eulerAngles.y == 0.0f)
        {
            // rotate the player to look left
            gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                55,
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
            DetectHit();

            // update the player's ammo count
            ammo--;
        }

        // setup flags
        isAttacking = true;
    }

    // called when a zombie attacks the player
    public void Hurt(int damage)
    {
        isHurt = true;
        hurtTime = Time.time;
        health -= damage;
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
        DetectHit();
    }

    // detect if we hit a zombie
    void DetectHit()
    {
        // check that we're supposed to be here
        if (detectingHit) return;
        detectingHit = true;

        Debug.DrawRay(shotLocation.transform.position, shotLocation.transform.forward, Color.red, 9999.0f, false);

        // check if this is a gunshot from the player's hand
        RaycastHit hit;
        if (weaponType != weaponTypes.MELEE && Physics.Raycast(shotLocation.transform.position, shotLocation.transform.forward, out hit))
        {
            // check if we hit a zombie
            if (hit.collider.gameObject.CompareTag("zombie")) DamageZombie(hit.collider.gameObject);
            else Destroy(Instantiate(dirtSplatter, hit.transform), 1.0f);
        }

        // otherwise check if this is a melee attack from the player's body
        else if (weaponType == weaponTypes.MELEE && Physics.Raycast(meleePoint.transform.position, meleePoint.transform.forward, out hit))
        {
            if (hit.collider.gameObject.CompareTag("zombie"))
            {
                if (hit.distance < 3)
                {
                    DamageZombie(hit.collider.gameObject);

                    // update the player's ammo count
                    ammo--;
                }
            }
        }

        Debug.DrawRay(meleePoint.transform.position, meleePoint.transform.forward, Color.red, 9999.0f, false);
    }

    // deals damage to the first zombie in range of attack
    void DamageZombie(GameObject zombieObj)
    {
        Zombie zombie = zombieObj.GetComponent<Zombie>();
        switch (weaponType)
        {
            case weaponTypes.MELEE:
                zombie.Hurt(100);
                break;
            case weaponTypes.SHOTGUN:
                zombie.Hurt(50);
                break;
            case weaponTypes.ASSAULT:
                zombie.Hurt(34);
                break;
            case weaponTypes.MINIGUN:
                zombie.Hurt(25);
                break;
            case weaponTypes.PISTOL:
                zombie.Hurt(20);
                break;
        }
    }

    public void EquipWeapon(string type, int ammo)
    {
        // equp the player's new weapon
        switch (type)
        {
            case "Shotgun":
                // equip the player's shotgun
                equip.ShotgunWeapon();
                animator.SetInteger("WeaponType_int", 4);
                weaponType = weaponTypes.SHOTGUN;
                this.ammo = ammo;
                break;
            case "Melee":
                // equip the player's melee weapon
                equip.HandWeapon();
                animator.SetInteger("WeaponType_int", 12);
                weaponType = weaponTypes.MELEE;
                this.ammo = ammo;
                break;
            case "Minigun":
                // equip the player's minigun
                equip.MinigunWeapon();
                animator.SetInteger("WeaponType_int", 9);
                weaponType = weaponTypes.MINIGUN;
                this.ammo = ammo;
                break;
            case "Assault":
                // equip the player's assault gun
                equip.ARWeapon();
                animator.SetInteger("WeaponType_int", 2);
                weaponType = weaponTypes.ASSAULT;
                this.ammo = ammo;
                break;
            case "Health":
                // update the player's health
                if (ammo >= 100) health = 100;
                else health += ammo;
                break;
            case "Ammo":
                // update the player's ammo
                this.ammo += ammo;
                break;
        }

        Debug.Log("Equipped \"" + type + "\".");
    }
}
