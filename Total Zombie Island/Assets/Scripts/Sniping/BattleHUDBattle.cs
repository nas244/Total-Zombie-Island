using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUDBattle : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public Gradient gradient;
    public Image fill;

    public void setHUD(Unit unit)
    {
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        fill.color = gradient.Evaluate(1f);

    }

    public void SetHP(float hp)
    {
        hpSlider.value = hp;
        fill.color = gradient.Evaluate(hpSlider.normalizedValue);
    }
}
