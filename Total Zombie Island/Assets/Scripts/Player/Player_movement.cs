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

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _characterController = this.GetComponent<CharacterController>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
        }

        movement *= speed;
        movement.y -= 20.0f * Time.deltaTime;

        Main_Character.transform.Rotate(0, Input.GetAxis("Horizontal") * turningspeed * Time.deltaTime, 0);


        


        _characterController.Move(movement * Time.deltaTime);
    }
}
