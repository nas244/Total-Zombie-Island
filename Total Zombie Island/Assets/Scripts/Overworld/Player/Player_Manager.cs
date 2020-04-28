using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public static Player_Manager instance;

    public GameObject Main_Character;

    private void Awake()
    {
        instance = this;
    }
}
