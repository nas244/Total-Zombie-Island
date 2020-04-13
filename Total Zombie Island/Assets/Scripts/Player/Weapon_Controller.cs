using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour
{
    [SerializeField]
    private float _damage = 50.0f;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _range = 50.0f;
    [SerializeField]
    private float _impactForce = 100.0f;
    [SerializeField]
    private Transform _shotLocation;

    private float _nextShotTime;
    [SerializeField]
    private float _shotAnimDelayTime = 0.09f;

    [SerializeField]
    private GameObject _muzzleFlash;
    private Renderer _flashRenderer;
    [SerializeField]
    private Light _flashLight;
    [SerializeField]
    private Light _muzzleFlashLight;

    [SerializeField]
    private GameObject _bloodSplatter;
    [SerializeField]
    private GameObject _impactSpark;

    [SerializeField]
    private AudioSource _gunShot;

    [SerializeField]
    private int _weaponAnim = 1;
    [SerializeField]
    private bool _fullAuto = false;
    private Animator _animator;

    [SerializeField]
    private LineRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _nextShotTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    public IEnumerator Fire(Vector3 direction)
    {
        if (Time.time < _nextShotTime) yield break;

        _nextShotTime = Time.time + _fireRate;
        _animator.SetBool("Shoot_b", true);

        yield return new WaitForSeconds(_shotAnimDelayTime);

        _gunShot.PlayOneShot(_gunShot.clip, 0.5f);

        RaycastHit hit;

        Vector3 angles = _shotLocation.eulerAngles;
        angles.x = 0;
        angles.z = 0;

        _shotLocation.eulerAngles = angles;
        
        if(Physics.Raycast(_shotLocation.position, _shotLocation.forward, out hit, _range))
        {
            //Debug.DrawRay(_shotLocation.position, _shotLocation.forward * hit.distance, Color.yellow);
            GameObject effect;

            if (hit.transform.CompareTag("zombie"))
            {
                effect = Instantiate(_bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
            }

            effect = Instantiate(_impactSpark, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(effect, 1.0f);
        }
        //Debug.Log("Hit Location:");
        //Debug.Log(hit.point);
        //Debug.Log("Firing Location:");
        //Debug.Log(_shotLocation.position);


        float dist = _range;

        if (hit.point != null)
        {
            dist = Vector3.Distance(_shotLocation.position, hit.point);
        }
        dist = Mathf.Min(_range, dist);

        float pos1OffsetDistance = Random.Range(0, dist - dist/5.0f);
        Vector3 pos1 = _shotLocation.position + direction * pos1OffsetDistance;

        float remaining = dist - pos1OffsetDistance;
        float pos2OffsetDistance = Random.Range(dist / 5.0f, Mathf.Min(dist / 2.0f, remaining));

        Vector3 pos2 = pos1 + direction * pos2OffsetDistance;

        _lineRenderer.SetPosition(0, pos1);
        _lineRenderer.SetPosition(1, pos2);

        _lineRenderer.enabled = true;
        yield return new WaitForEndOfFrame();
        _lineRenderer.enabled = false;
    }

    public void Stop()
    {
        _animator.SetBool("Shoot_b", false);
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            this.enabled = true;

            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }

            _lineRenderer.enabled = false;

            _animator.SetInteger("WeaponType_int", _weaponAnim);
            _animator.SetBool("FullAuto_b", _fullAuto);
        }
        else
        {
            this.enabled = false;
            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
           
        }
    }
}

