using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CharacterController))]
public class Player_movement : MonoBehaviour
{
    public static Player_movement instance;
    bool canBePlayed = true;

    [SerializeField]
    private Animator _animator;

    private float _health;

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

    private float _stamina;

    private bool _reloading;

    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private Camera _carCamera;

    private GameObject _character;

    public bool _inVehicle;

    [SerializeField]
    private GameObject _vehicle;

    public int _currentObjective;

    [SerializeField]
    private GameObject _scoreObj;

    private Score_Controller _scoreCtrl;

    [SerializeField]
    private GameObject _weaponwheel;

    private WeaponSwitching _wheelCtrl;

    public DialogueSystem System;
    //PauseMenu pauseMenu;

    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();

        _animator = this.GetComponent<Animator>();
        _characterController = this.GetComponent<CharacterController>();
        _playerCamera.gameObject.gameObject.SetActive(true);
        _carCamera.gameObject.gameObject.SetActive(false);
        _character = GameObject.Find("SA_Char_Survivor_HoodedMan");
        _scoreCtrl = _scoreObj.GetComponent<Score_Controller>();
        _inVehicle = false;
        _wheelCtrl = _weaponwheel.GetComponent<WeaponSwitching>();

        _characterController.enabled = false;
        _health = State_Data.Instance._health;
        _stamina = State_Data.Instance._stamina;
        Main_Character.transform.position = State_Data.Instance._position;
        Main_Character.transform.rotation = State_Data.Instance._rotation;
        _currentObjective = State_Data.Instance._currentObjective;
        _scoreCtrl._rating = State_Data.Instance._score;
        _characterController.enabled = true;

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

        // disable audio
        foreach (GameObject weapon in _weapons)
        {
            Weapon_Controller ctrl = weapon.GetComponent<Weapon_Controller>();
            Debug.Log(weapon);
            ctrl.SetIndex(index);
            ctrl.SetAnimator(_animator);
            ctrl.SetActive(false);
            index += 1;
        }

        // re enable audio

        _weaponCtrl = _weapons[_nextWeapon].GetComponent<Weapon_Controller>();
        _weaponCtrl.SetActive(true);

        _reloading = true;       

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (!System.isTyping && !NPC_AI.pickingOption)
            {
                if (Input.GetKey("escape"))
                {
                    Application.Quit();
                }
                if (_vehicleCtrl == null)
                {
                    if(!_wheelCtrl.SlowDownGame)
                    {
                        Movement();
                    }
                    Weapons();
                    WeaponSwitch();
                }
                InteractUpdate();
            }
        }
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
            if (canBePlayed) { canBePlayed = false; SoundManager.PlaySound(SoundManager.Sound.Jump); }
            movement.y = jumping;
            _animator.SetBool("Jump_b", true);
        }
        else
        {
            _animator.SetBool("Jump_b", false);
            canBePlayed = true;
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
                _stamina += .25f;
            }
        }
        
        movement.y -= 20.0f * Time.deltaTime;

        Main_Character.transform.Rotate(0, Input.GetAxis("Mouse X") * speed, 0);       

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
                    SoundManager.PlaySound(SoundManager.Sound.Reload);
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
                    if (collider.CompareTag("Health"))
                    {
                        Heal();
                    }
                
                    var car = collider.GetComponent<vehicle_controller>();
                    if(car != null)
                    {
                        _characterController.enabled = false;
                        _playerCamera.gameObject.SetActive(false);
                        _carCamera.gameObject.SetActive(true);
                        _character.SetActive(false);
                        _vehicleCtrl = car;
                        _inVehicle = true;
                    }
                }
            }
        }
        else
        {
            Vector3 teleport = new Vector3(_vehicle.transform.position.x + 3, _vehicle.transform.position.y, _vehicle.transform.position.z);
            this.transform.position = teleport;
            if(Input.GetKeyDown(KeyCode.E))
            {
                _characterController.enabled = true;
                _inVehicle = false;
                _playerCamera.gameObject.SetActive(true);
                _carCamera.gameObject.SetActive(false);
                _character.SetActive(true);
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

    public void Save_Data()
    {
        State_Data.Instance._health = _health;
        State_Data.Instance._stamina = _stamina;
        State_Data.Instance._position = Main_Character.transform.position;
        State_Data.Instance._rotation = Main_Character.transform.rotation;
        State_Data.Instance._currentObjective = _currentObjective;
        State_Data.Instance._score = _scoreCtrl._rating;
    }

    public void Reset_Data()
    {
        State_Data.Instance._health = 100f;
        State_Data.Instance._stamina = 50f;
        State_Data.Instance._position = new Vector3(0, 0, -5.42f);
        State_Data.Instance._rotation = new Quaternion(0, 0, 0, 0);
        State_Data.Instance._currentObjective = -1;
    }

    private void Heal()
    {
        _health = 100;
    }
}