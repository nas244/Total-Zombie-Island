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

    public GameObject MainChar;


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
        Hand.gameObject.SetActive(false);
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
        Hand.gameObject.SetActive(false);
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
        Hand.gameObject.SetActive(false);
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
        Hand.gameObject.SetActive(false);
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
        Hand.gameObject.SetActive(false);
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
        Hand.gameObject.SetActive(false);
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
        Hand.gameObject.SetActive(false);
    }




}
