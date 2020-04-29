using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameSequence : MonoBehaviour
{

    public DialogueSystem System;
    public ThemeManager Theme;

    [SerializeField]
    GameObject DialogueHUD;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fuck Unity");
        System = System.GetComponent<DialogueSystem>();

        DialogueHUD.SetActive(false);

        if (State_Data.Instance._currentObjective == 3)
        {
            Debug.Log("Fuck Unity + 1");
            Theme.DisableAll();
            Theme.SetTheme();

            AudioListener.pause = true;
            StartCoroutine(EndSequence());
        }

        Debug.Log("Fuck Unity + 2");
    }

    IEnumerator EndSequence()
    {
        Debug.Log("End Sequence");
        yield return new WaitForSeconds(1);

        string[] sentence = new string[]
        {
            "Announcer: Seems you have reached the end",
            "Announcer: . . .",
            "Announcer: Awesome! Begin Ending"
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
    }

}
