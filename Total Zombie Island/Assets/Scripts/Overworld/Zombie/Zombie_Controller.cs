using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Zombie_Controller : MonoBehaviour
{
    private float _health;

    private float _runSpeed;

    [SerializeField]
    private float _walkSpeed = 2.0f;

    private float _attackDamage;

    [SerializeField]
    private float _deadDespawnRange = 60.0f;
    [SerializeField]
    private float _despawnRange = 150.0f;

    [SerializeField]
    private float _lookRadius = 20.0f;
    
    [SerializeField]
    private float _chaseLookRadius = 35.0f;
    
    [SerializeField]
    private float _fov = 90.0f;
    
    [SerializeField]
    private float _rotationSpeed = 5.0f;
    
    [SerializeField]
    private float _attackDelay = 1.0f;
    
    private float _nextAttackTime;

    private Vector3 _targetPos;
    
    private Transform _playerPos;
    
    private Player_movement _playerCtrl;

    private Animator _animator;

    private UnityEngine.AI.NavMeshAgent _agent;

    private bool _dead = false;

    public GameObject MainChar;

    public GameObject Score;

    public Score_Controller _scoreCtrl;

    // zombie sounds
    public AudioSource _audioSrc;
    public AudioClip[] _audioClips;

    enum ZombieState
    {
        Patrol,
        Attacking
    }

    ZombieState _currentState = ZombieState.Patrol;
    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();
        _animator = this.GetComponent<Animator>();
        _agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        MainChar = GameObject.Find("Main_Character");
        _playerPos = MainChar.transform;
        _playerCtrl = MainChar.GetComponent<Player_movement>();
        Score = GameObject.Find("Score");
        _scoreCtrl = Score.GetComponent<Score_Controller>();

        _nextAttackTime = Time.time;

        _health = 100f;
        _runSpeed = 5.0f;
        _attackDamage = 10f;

        _agent.speed = _walkSpeed;

        _targetPos = this.transform.position;

        if (!_agent.isOnNavMesh) Destroy(this.gameObject);

        if (State_Data.Instance._currentObjective >= 2)
        {
            _walkSpeed += 1.0f;
            _lookRadius += 5f;
            _chaseLookRadius += 5f;
            _runSpeed += 1;
            _health += 10;
        }

        // kick off the sound coroutine
        StartCoroutine(Audio());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        Despawn();
    }

    public IEnumerator Audio()
    {
        // pick a random time delay to play sound
        yield return new WaitForSeconds(Random.Range(3, 16));

        // if this zombie isn't dead
        if (!_dead)
        {
            // play a random sound
            _audioSrc.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Length - 1)]);

            // call this coroutine again to keep the sounds going
            StartCoroutine(Audio());
        }
    }

    private void Movement()
    {
        if (!_agent.enabled) return;

        float playerDist = 0.0f;

        switch(_currentState)
        {
            case ZombieState.Patrol:
                if ((State_Data.Instance._currentObjective == 3 || State_Data.Instance._currentObjective == 2) && playerDist < _lookRadius)
                {
                    _currentState = ZombieState.Attacking;
                    _agent.SetDestination(_playerPos.position);
                    _agent.speed = _runSpeed;
                    break;
                }

                float targetDist = Vector3.Distance(this.transform.position, _targetPos);

                if (targetDist <= _agent.stoppingDistance)
                {
                    float newX = Random.Range(-_lookRadius - 5, _lookRadius - 5);
                    float newZ = Random.Range(-_lookRadius - 5, _lookRadius - 5);

                    _targetPos.x += newX;
                    _targetPos.y = this.transform.position.y;
                    _targetPos.z += newZ;

                    _agent.SetDestination(_targetPos);
                }

                playerDist = Vector3.Distance(this.transform.position, _playerPos.position);

                if (playerDist <= _lookRadius)
                {
                    Vector3 toPlayer = (_playerPos.position - this.transform.position).normalized;
                    float angle = Mathf.Abs(Vector3.Angle(this.transform.forward, toPlayer));

                    if (angle <= _fov / 2.0)
                    {
                        _currentState = ZombieState.Attacking;
                        _agent.SetDestination(_playerPos.position);
                        _agent.speed = _runSpeed;
                    }
                }
                break;

            case ZombieState.Attacking:
                _agent.SetDestination(_playerPos.position);

                //playerDist = Vector3.Distance(this.transform.position, _playerPos.position);
                playerDist = Vector3.Distance(transform.position, _playerPos.position);

                if (playerDist <= _agent.stoppingDistance)
                {
                    //Vector3 direction = (_playerPos.position - this.transform.position).normalized;
                    Vector3 direction = (_playerPos.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);

                    if (Time.time > _nextAttackTime)
                    {
                        _nextAttackTime = Time.time + _attackDelay;
                        if(!_playerCtrl._inVehicle && State_Data.Instance._canBeHit)
                        {
                            StartCoroutine(_playerCtrl.Damage(_attackDamage));
                        }
                        float playerhealth = _playerCtrl.GetHealth();
                        
                        if(playerhealth <= -10)
                        {
                            _animator.Play("Zombie_Eating");
                        }
                    }
                }
                else if (playerDist > _chaseLookRadius + 3)
                {
                    _currentState = ZombieState.Patrol;
                    _agent.speed = _walkSpeed;

                    float newX = Random.Range(-_lookRadius - 5, _lookRadius - 5);
                    float newZ = Random.Range(-_lookRadius - 5, _lookRadius - 5);

                    _targetPos.x += newX;
                    _targetPos.y = this.transform.position.y;
                    _targetPos.z += newZ;

                    _agent.SetDestination(_targetPos);
                }
                break;
        }


    }

    private void Despawn()
    {
        float dist = Vector3.Distance(this.transform.position, _playerPos.position);

        if (_health <= 0 && dist >= _deadDespawnRange) Destroy(this.gameObject);

        //else if (dist >= _despawnRange) Destroy(this.gameObject);
    }

    public void Damage(float damage)
    {
        if (_dead) return;

        Debug.Log("Hit Zombie");

        _health -= damage;

        _currentState = ZombieState.Attacking;

        if (_agent.enabled)
        {
            _agent.SetDestination(_playerPos.position);
            _agent.speed = _runSpeed;
        }

        if (_health <= 0)
        {
            _dead = true;
            EnableRagdoll();
            _scoreCtrl.inc_rating(0.1f);
        }
    }

    private void DisableRagdoll()
    {
        Rigidbody[] rigidBodies = this.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
        }
    }

    private void EnableRagdoll()
    {
        _animator.enabled = false;
        _agent.enabled = false;

        Rigidbody[] rigidBodies = this.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }
    }
}
