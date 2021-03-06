﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Data : MonoBehaviour
{
    public static State_Data Instance;

    public float _health;
    public float _stamina;
    public Vector3 _position;
    public Quaternion _rotation;
    public int _currentObjective;
    public float _score;
    public float _scoreCap = 2;
    public int _spawnLimit;
    public float _spawnDelay;
    //public bool _setHector;
    public bool _canBeHit = true;
    public int _hectorDialogue = 0;
    // new stuff
    public bool _MG1Complete = false, _MG2Complete = false, _MG3Complete = false;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _health = 100f;
         _stamina = 50f;
        _position = new Vector3 (0,0, -5.42f);
        _rotation = new Quaternion(0,0,0,0);
        //_currentObjective = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
