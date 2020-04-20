using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // function called when the game is over
    public void GameOver(bool victory)
    {
        // if the player won, log to console victory
        if (victory) Debug.Log("Game Over: Victory!");
        else Debug.Log("Game Over: You died :(");
    }
}
