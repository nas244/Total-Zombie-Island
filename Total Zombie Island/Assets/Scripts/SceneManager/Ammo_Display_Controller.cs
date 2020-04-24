using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo_Display_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    private Player_movement _playerCtrl;

    [SerializeField]
    private Text _ammoText;
    // Start is called before the first frame update
    void Start()
    {
        _playerCtrl = character.GetComponent<Player_movement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        int ammo = _playerCtrl._weaponCtrl._currentAmmo;
        int clipnum = _playerCtrl._weaponCtrl._currentClipNum;
        _ammoText.text = "Current Clip: " + ammo.ToString() + "\n" + "Number of Clips:" + clipnum.ToString();
    }
}
