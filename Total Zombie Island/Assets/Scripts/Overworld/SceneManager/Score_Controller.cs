﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_Controller : MonoBehaviour
{
    public float _rating;
    private float _onscreen;

    public Slider scoreSlider;

    [SerializeField]
    private Text _ratingText;
    // Start is called before the first frame update
    void Start()
    {
        //scoreSlider.maxValue = 5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rating -= 0.00001f;
        _onscreen = _rating;
        if (_onscreen > 5.0f)
        {
            _onscreen = 5.0f;
        }

        setScore(_onscreen);
        //_ratingText.text = "Score: " + _onscreen.ToString("F1");

        if (_rating <= 0 )
        {
            //Trigger GameOver
        }
    }

    public void setScore(float score)
    {
        scoreSlider.value = score;
    }

    public void inc_rating(float reward)
    {
        _rating += reward;
    }
}
