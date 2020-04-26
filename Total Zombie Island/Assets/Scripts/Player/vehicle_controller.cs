using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicle_controller : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _wheel;
    [SerializeField]
    private WheelCollider[] _wheelColliders;
    [SerializeField]
    private GameObject _mainChar;

    private Player_movement _playerCtrl;

    private List<WheelCollider> _wheelcollider;

    private float _topSpeed = 400f;
    private float _maxTorque = 400f;
    private float _maxSteerAngle = 60f;
    private float _maxBrakeTorque = 2200f;

    private float _forward;
    private float _turn;
    private float _brake;

    private Rigidbody _body;

    

    // Start is called before the first frame update
    void Start()
    {
        _body = this.GetComponent<Rigidbody>();
        _wheelcollider = new List<WheelCollider>();
        foreach (WheelCollider wheel in _wheelColliders)
        {
            _wheelcollider.Add(wheel.GetComponent<WheelCollider>());
        }

        _playerCtrl = _mainChar.GetComponent<Player_movement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_playerCtrl._inVehicle)
        {
            _forward = Input.GetAxis("Vertical");
            _turn = Input.GetAxis("Horizontal");
            _brake = Input.GetAxis("Jump");

            _wheelcollider[2].steerAngle = _maxSteerAngle * _turn;
            _wheelcollider[3].steerAngle = _maxSteerAngle * _turn;

            float currentSpeed = 2 * 22 / 7 * _wheelcollider[0].radius * _wheelcollider[0].rpm * 60 / 1000;

            if (currentSpeed < _topSpeed)
            {
                _wheelcollider[0].motorTorque = _maxTorque * _forward;
                _wheelcollider[1].motorTorque = _maxTorque * _forward;
            }

            _wheelcollider[0].brakeTorque = _maxBrakeTorque * _brake;
            _wheelcollider[1].brakeTorque = _maxBrakeTorque * _brake;
            _wheelcollider[2].brakeTorque = _maxBrakeTorque * _brake;
            _wheelcollider[3].brakeTorque = _maxBrakeTorque * _brake;
        }
    }

    void Update()
    {
        if (_playerCtrl._inVehicle)
        {
            Quaternion flq;
            Vector3 flv;
            _wheelcollider[2].GetWorldPose(out flv, out flq);
            _wheel[2].transform.position = flv;
            _wheel[2].transform.rotation = flq;

            Quaternion blq;
            Vector3 blv;
            _wheelcollider[0].GetWorldPose(out blv, out blq);
            _wheel[0].transform.position = blv;
            _wheel[0].transform.rotation = blq;

            Quaternion frq;
            Vector3 frv;
            _wheelcollider[3].GetWorldPose(out frv, out frq);
            _wheel[3].transform.position = frv;
            _wheel[3].transform.rotation = frq;

            Quaternion brq;
            Vector3 brv;
            _wheelcollider[1].GetWorldPose(out brv, out brq);
            _wheel[1].transform.position = brv;
            _wheel[1].transform.rotation = brq;
        }
    }
}
