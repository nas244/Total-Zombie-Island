using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CharacterController))]
public class Player_movement : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private float _health = 100;

    private bool _isDead = false;

    private CharacterController _characterController;

    public GameObject Main_Character;

    [SerializeField]
    private GameObject _weaponRoot;
    [SerializeField]
    private GameObject[] _weaponPrefabs;

    private List<GameObject> _weapons;
    private int _currentWeapon = 0;
    public int _nextWeapon = 0;
    public Weapon_Controller _weaponCtrl;

    private vehicle_controller _vehicleCtrl;

    private float _stamina = 100f;

    private bool _reloading;

    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private Camera _carCamera;

    private GameObject _character;
    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _characterController = this.GetComponent<CharacterController>();
        _playerCamera.gameObject.gameObject.SetActive(true);
        _carCamera.gameObject.gameObject.SetActive(false);
        _character = GameObject.Find("SA_Char_Survivor_HoodedMan");

        _weapons = new List<GameObject>();
        _weapons.Add(Instantiate(_weaponPrefabs[0], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[1], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[2], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[3], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[4], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[5], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[6], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[7], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[8], _weaponRoot.transform));

        int index = 0;

        foreach (GameObject weapon in _weapons)
        {
            Weapon_Controller ctrl = weapon.GetComponent<Weapon_Controller>();
            Debug.Log(weapon);
            ctrl.SetIndex(index);
            ctrl.SetAnimator(_animator);
            ctrl.SetActive(false);
            index += 1;
        }

        _weaponCtrl = _weapons[_nextWeapon].GetComponent<Weapon_Controller>();
        _weaponCtrl.SetActive(true);

        _reloading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("Sniping");
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (_vehicleCtrl == null)
        {
            Movement();
            Weapons();
            WeaponSwitch();
        }
        InteractUpdate();
    }

    private void Movement()
    {
        float speed = 5.0f;
        float running = 10f;
        float turningspeed = 240f;
        float jumping = 1.0f;

        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        Vector3 movement = Vector3.zero;

        if (moveDirection != Vector3.zero)
        {
            float setspeed;
            if (Input.GetKey(KeyCode.LeftShift) && _stamina > 0)
            {
                setspeed = 1.0f;
            }
            else
            {
                setspeed = .49f;
            }
            
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

        if (Input.GetKey(KeyCode.LeftShift) && _stamina > 0)
        {
            movement *= running;
            _stamina -= .5f;
        }
        else
        {
            movement *= speed;
            if (_stamina < 100f)
            {
                _stamina += .05f;
            }
        }
        
        movement.y -= 20.0f * Time.deltaTime;

        Main_Character.transform.Rotate(0, Input.GetAxis("Horizontal") * turningspeed * Time.deltaTime, 0);


        


        _characterController.Move(movement * Time.deltaTime);
    }

    private void Weapons()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.R))
        {
            if (_weaponCtrl._currentAmmo <= 0 || Input.GetKey(KeyCode.R))
            {
                if (_reloading)
                {
                    _reloading = false;
                    StartCoroutine(_weaponCtrl.Reload());
                }
                
            }
            else 
            {
                _reloading = true;
                StartCoroutine(_weaponCtrl.Fire(this.transform.forward));
            }
            
        }
        else
        {
            _weaponCtrl.Stop();
        }
    }

    private void InteractUpdate()
    {
        if(_vehicleCtrl == null)
        {
            var colliders = Physics.OverlapSphere(Main_Character.transform.position, 5f);
            foreach (var collider in colliders)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (collider.CompareTag("Ammo_Box"))
                    {
                        _weaponCtrl.Refill();
                    }
                
                    var car = collider.GetComponent<vehicle_controller>();
                    if(car != null)
                    {
                        _playerCamera.gameObject.SetActive(false);
                        _carCamera.gameObject.SetActive(true);
                        _character.SetActive(false);
                        _vehicleCtrl = car;
                        //Main_Character.t
                    }
                }
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _vehicleCtrl = null;
            }
        }
    }

    private void WeaponSwitch()
    {
        if(_nextWeapon != _currentWeapon)
        {
            Debug.Log("Kill me");
            _weaponCtrl.SetActive(false);

            _currentWeapon = _nextWeapon;

            _weaponCtrl = _weapons[_currentWeapon].GetComponent<Weapon_Controller>();

            _weaponCtrl.SetActive(true);
        }
    }

    public IEnumerator Damage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            this.enabled = false;
            _animator.SetBool("Death_b", true);

            yield return new WaitForSeconds(5.0f);

            _isDead = true;

            SceneManager.LoadScene("Overworld");
        }

        yield break;
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetStamina()
    {
        return _stamina;
    }
}