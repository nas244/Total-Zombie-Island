﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameSequence : MonoBehaviour
{

    public DialogueSystem System;
    public ThemeManager Theme;
    public LevelLoader Loader;

    [SerializeField]
    GameObject DialogueHUD, CountdownHUD, ObjectiveHUD, ScoreHUD, EndObject, PlayerObject, SpotLight;

    Transform EndLocation;

    [SerializeField]
    int count = 120, lookRadius = 10, interactRadius = 5;

    [SerializeField]
    TextMeshProUGUI countdownText;

    // Start is called before the first frame update
    void Start()
    {
        System = System.GetComponent<DialogueSystem>();
        EndLocation = EndObject.GetComponent<Transform>();

        DialogueHUD.SetActive(false);
        CountdownHUD.SetActive(false);
        SpotLight.SetActive(false);

        if (State_Data.Instance._currentObjective == 3)
        {
            Theme.DisableAll();
            Theme.SetTheme();

            AudioListener.pause = true;
            StartCoroutine(EndSequence());
        }
    }

    void Update()
    {
        if ((EndLocation.transform.position - PlayerObject.transform.position).sqrMagnitude < lookRadius * lookRadius)
        {
            //Debug.Log("within range");
            //iconObject.SetActive(true);

            // Rotate the enemy too always face the player.
            Vector3 targetPosition = new Vector3(PlayerObject.transform.position.x, EndLocation.transform.position.y, EndLocation.transform.position.z);
            //transform.LookAt(targetPosition);
            //iconObject.transform.LookAt(targetPosition);

            if ((EndLocation.transform.position - PlayerObject.transform.position).sqrMagnitude < interactRadius * interactRadius)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Fuuuuuuuuuuck .... UNITY");

                    Loader.LoadLevel("Winner");
                }
            }
        }
    }

    IEnumerator EndSequence()
    {
        Debug.Log("End Sequence");
        yield return new WaitForSeconds(1);

        string[] sentence = new string[]
        {
            "Hector: . . .",
            "Hector: I - I don't believe it!",
            "Hector: (this wasn't supposed to happen)",
            "Hector: *ahem* This is an unexpected turn of events folks",
            "Hector: Who would have thought Dave could ever be defeated like that?",
            "Hector: I surely didn't!",
            "Hector: Ugh, well we are out of minigames...",
            "Hector: I must commend the contestant,",
            "Hector: You vastly exceeded my expectations",
            "Hector: How bout we up the ante a bit eh folks?",
            "Hector: If you can make it to the lighthouse before you explode . . .",
            "Hector: We'll take you off the island! Sound fair?",
            "Hector: No? Well I've already made up my mind. BEGIN COUNTDOWN!"
        };

        System.sentenceArray.Add(sentence);

        DialogueHUD.SetActive(true);
        System.StartDialogue();

        while (System.isTyping)
        {
            yield return null;
        }

        AudioListener.pause = false;
        DialogueHUD.SetActive(false);
        ObjectiveHUD.SetActive(false);
        ScoreHUD.SetActive(false);
        CountdownHUD.SetActive(true);
        SpotLight.SetActive(true);

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (count != 0)
        {
            // if player has reached goal yield break
            // else
            count--;
            countdownText.text = "TIME UNTIL DETONATION: " + count;
            yield return new WaitForSeconds(1);
        }

        Loader.LoadLevel("GameOver");
        // trigger game over
    }

}
