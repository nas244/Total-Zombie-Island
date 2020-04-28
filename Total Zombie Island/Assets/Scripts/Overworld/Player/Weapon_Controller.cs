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

    public int _clipSize;

    [SerializeField]
    private int _clipNum;
    public int _currentAmmo;
    public int _currentClipNum;

    [SerializeField]
    private LineRenderer _lineRenderer;

    private int _weaponIndex;

    bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        _nextShotTime = Time.time;
        _currentAmmo = _clipSize;
        _currentClipNum = _clipNum;
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

        if (isReloading) yield break;

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


            if (_weaponIndex == 5 || _weaponIndex == 7)
            {
                effect = Instantiate(_bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));

                var colliders = Physics.OverlapSphere(hit.transform.position, 10f);

                foreach (var collider in colliders)
                {
                    
                    var zomb = collider.GetComponent<Zombie_Controller>();
                    if (zomb != null)
                    {
                        zomb.Damage(_damage);
                    }
                    
                    
                }
            }
            else if (hit.transform.CompareTag("zombie"))
            {
                hit.transform.gameObject.GetComponentInParent<Zombie_Controller>().Damage(_damage);
                Debug.Log(hit.transform.gameObject);

                hit.rigidbody.AddForce(-hit.normal * _impactForce, ForceMode.Impulse);

                effect = Instantiate(_bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                effect = Instantiate(_impactSpark, hit.point, Quaternion.LookRotation(hit.normal));
            }
            
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

        _currentAmmo -= 1;
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

    public void SetIndex(int index)
    {
        _weaponIndex = index;
    }

    public IEnumerator Reload()
    {
        if(_currentClipNum > 0)
        {
            isReloading = true;

            _animator.SetBool("Reload_b", true);

            // Changed how this works so you don't lose ammo on reload
            while (_currentAmmo < _clipSize)
            {
                if (_currentClipNum == 0) { break; }

                _currentAmmo++;
                _currentClipNum --;
            }

            yield return new WaitForSeconds(1);

            //_currentAmmo = _clipSize;
            //_currentClipNum -= 1;
            _animator.SetBool("Reload_b", false);

            yield return new WaitForSeconds(0.75f);
            isReloading = false;
            yield break;
        }

    }

    public void Refill()
    {
        _currentClipNum = _clipNum;
    }
}

