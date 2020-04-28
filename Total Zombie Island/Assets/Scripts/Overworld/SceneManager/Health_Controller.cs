using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health_Controller : MonoBehaviour
{
    //[SerializeField]
    //private Text _healthText;

    private float _health;
    private float _stamina;

    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public Gradient gradient;
    public Image fill;
    public bool isSet = false;

    private Player_movement _playerCtrl;

    public GameObject MainChar;
    // Start is called before the first frame update
    void Start()
    {
        MainChar = GameObject.Find("Main_Character");
        _playerCtrl = MainChar.GetComponent<Player_movement>();
        _health = _playerCtrl.GetHealth();
        //Debug.Log(_health);
        //setHUD(_health);
    }

    // Update is called once per frame
    void Update()
    {
        _health = _playerCtrl.GetHealth();
        _stamina = _playerCtrl.GetStamina();
        //Debug.Log(_health);
        if (!isSet) { setHUD(_health); isSet = true; }
        else { SetHP(_health); }
        //_healthText.text = "Health: " + _health.ToString() + "\n" + "Stamina: " + _stamina.ToString("0");
    }

    public void setHUD(float health)
    {
        hpSlider.maxValue = health;
        hpSlider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHP(float hp)
    {
        hpSlider.value = hp;
        fill.color = gradient.Evaluate(hpSlider.normalizedValue);
    }
}
