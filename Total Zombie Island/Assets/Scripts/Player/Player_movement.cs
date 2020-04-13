using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]
public class Player_movement : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private CharacterController _characterController;

    private Camera _mainCamera;

    public GameObject Main_Character;

    [SerializeField]
    private GameObject _weaponRoot;
    [SerializeField]
    private GameObject[] _weaponPrefabs;

    private List<GameObject> _weapons;
    private int _currentWeapon = 0;
    private Weapon_Controller _weaponCtrl;

    private vehicle_controller _vehicleCtrl;
    

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _characterController = this.GetComponent<CharacterController>();
        _mainCamera = Camera.main;

        _weapons = new List<GameObject>();
        _weapons.Add(Instantiate(_weaponPrefabs[0], _weaponRoot.transform));

        foreach(GameObject weapon in _weapons)
        {
            Weapon_Controller ctrl = weapon.GetComponent<Weapon_Controller>();
            Debug.Log("SetActive");
            ctrl.SetAnimator(_animator);
            ctrl.SetActive(false);

        }

        _weaponCtrl = _weapons[0].GetComponent<Weapon_Controller>();
        _weaponCtrl.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (_vehicleCtrl == null)
        {
            Movement();
            Weapons();
        }
        vehicleupdate();
    }

    private void Movement()
    {
        float speed = 5.0f;
        float turningspeed = 180f;
        float jumping = 1.0f;

        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        Vector3 movement = Vector3.zero;

        if (moveDirection != Vector3.zero)
        {
            float setspeed;

            setspeed = .49f;

            _animator.SetFloat("Speed_f", setspeed);

            movement = Main_Character.transform.rotation * moveDirection;
        }
        else
        {
            _animator.SetFloat("Speed_f", 0.0f);
        }

        if (Input.GetButton("Jump"))
        {
            movement.y = jumping;
            _animator.SetBool("Jump_b", true);
        }
        else
        {
            _animator.SetBool("Jump_b", false);
        }

        movement *= speed;
        movement.y -= 20.0f * Time.deltaTime;

        Main_Character.transform.Rotate(0, Input.GetAxis("Horizontal") * turningspeed * Time.deltaTime, 0);


        


        _characterController.Move(movement * Time.deltaTime);
    }

    private void Weapons()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(_weaponCtrl.Fire(this.transform.forward));
        }
        else
        {
            _weaponCtrl.Stop();
        }
    }

    private void vehicleupdate()
    {
        if(_vehicleCtrl == null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var colliders = Physics.OverlapSphere(Main_Character.transform.position, 2f);
                foreach( var collider in colliders)
                {
                    var car = collider.GetComponent<vehicle_controller>();

                    if(car != null)
                    {
                        _vehicleCtrl = car;
                        Main_Character.transform.position = collider.transform.position;
                    }
                }
            }
        }
        else
        {
            Debug.Log("In car");
        }
    }
}