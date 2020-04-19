using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Controller : MonoBehaviour
{
    [SerializeField]
    private Text _healthText;

    private float _health;

    private Player_movement _playerCtrl;

    public GameObject MainChar;
    // Start is called before the first frame update
    void Start()
    {
        MainChar = GameObject.Find("Main_Character");
        _playerCtrl = MainChar.GetComponent<Player_movement>();
    }

    // Update is called once per frame
    void Update()
    {
        _health = _playerCtrl.GetHealth();
        _healthText.text = "Health: " + _health.ToString();
    }
}
