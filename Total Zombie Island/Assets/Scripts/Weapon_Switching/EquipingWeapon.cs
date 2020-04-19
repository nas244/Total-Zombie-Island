using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipingWeapon : MonoBehaviour
{
    public GameObject Shotgun;
    public GameObject SMG;
    public GameObject Hand;
    public GameObject SniperRifle;
    public GameObject GrenadeLauncher;
    public GameObject Minigun;
    public GameObject AssualtRifle;
    public GameObject RPG;
    public GameObject Pistol;



    [SerializeField]
    private Animator _anim;

    private CharacterController CharController;

    private Player_movement _playerctrl;

    public GameObject MainChar;


    void Start()
    {
        _playerctrl = MainChar.GetComponent<Player_movement>();
    }

    public void HandWeapon ()
    {
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Hand.gameObject.SetActive(true);

        _playerctrl._nextWeapon = 0;
    }
    public void ShotgunWeapon ()
    {
        _anim.SetInteger("WeaponType_int", 4);
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(true);

        _playerctrl._nextWeapon = 6;

    }
    public void SMGWeapon ()
    {
        _anim.SetInteger("WeaponType_int", 7);
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(false);
        Hand.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);
        SMG.gameObject.SetActive(true);

        _playerctrl._nextWeapon = 8;
    }

    public void PistolWeapon ()
    {
        _anim.SetInteger("WeaponType_int", 4);
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(true);
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);

        _playerctrl._nextWeapon = 3;

    }

    public void SniperWeapon ()
    {
        _anim.SetInteger("WeaponType_int", 4);
        SniperRifle.gameObject.SetActive(true);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);

        _playerctrl._nextWeapon = 2;
    }

    public void Glauncher()
    {
        _anim.SetInteger("WeaponType_int", 4);
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(true);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);

        _playerctrl._nextWeapon = 5;
    }

    public void ARWeapon()
    {
        _anim.SetInteger("WeaponType_int", 4);
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(true);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);

        _playerctrl._nextWeapon = 1;
    }

    public void MinigunWeapon()
    {
        _anim.SetInteger("WeaponType_int", 4);
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(true);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(false);
        Pistol.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);

        _playerctrl._nextWeapon = 4;
    }

    public void RPGWeapon()
    {
        _anim.SetInteger("WeaponType_int", 4);
        SniperRifle.gameObject.SetActive(false);
        GrenadeLauncher.gameObject.SetActive(false);
        Minigun.gameObject.SetActive(false);
        AssualtRifle.gameObject.SetActive(false);
        RPG.gameObject.SetActive(true);
        Pistol.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);

        _playerctrl._nextWeapon = 7;
    }




}
