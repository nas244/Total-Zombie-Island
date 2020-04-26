using System.Collections;
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
        _currentObjective = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
