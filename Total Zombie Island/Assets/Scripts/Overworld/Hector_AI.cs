using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hector_AI : MonoBehaviour
{
    [SerializeField]
    GameObject camObject, DialogueHUD;

    [SerializeField]
    string[] initialDialogue, obj1CompleteDialogue, obj2CompleteDialogue;

    //int timesTriggered = 0;

    Camera cam;

    public LevelLoader Loader;
    public DialogueSystem System;


    // Start is called before the first frame update
    void Start()
    {       
        cam = camObject.GetComponent<Camera>();
        System = System.GetComponent<DialogueSystem>();

        StartCoroutine(HectorDialogue());

        //DialogueHUD.SetActive(false);

        /*if (State_Data.Instance._setHector)
        {
            Debug.Log("tis true");
            State_Data.Instance._setHector = false;
            StartCoroutine(HectorDialogue());
        }
        else { camObject.SetActive(false); DialogueHUD.SetActive(false); }*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z));
    }

    IEnumerator HectorDialogue()
    {
        //State_Data.Instance._canBeHit = false;

        switch (State_Data.Instance._hectorDialogue)
        {
            case 0:
                Debug.Log("beginning of game");
                // begginning of game dialogue
                int length = initialDialogue.Length;
                int index = 0;
                string[] sentence = new string[length];

                while (index != length)
                {
                    sentence[index] = initialDialogue[index];
                    index++;
                }

                System.sentenceArray.Add(sentence);

                //DialogueHUD.SetActive(true);
                System.StartDialogue();

                while (System.isTyping)
                {
                    yield return null;
                }
                System.array++;
                break;

            case 1:
                Debug.Log("beat first minigame");
                // begginning of game dialogue
                int length2 = obj1CompleteDialogue.Length;
                int index2 = 0;
                string[] sentence2 = new string[length2];

                while (index2 != length2)
                {
                    sentence2[index2] = obj1CompleteDialogue[index2];
                    index2++;
                }

                System.sentenceArray.Add(sentence2);

                //DialogueHUD.SetActive(true);
                System.StartDialogue();

                while (System.isTyping)
                {
                    yield return null;
                }
                System.array++;
                break;

            case 3:
                Debug.Log("beat second minigame");
                // begginning of game dialogue
                int length3 = obj2CompleteDialogue.Length;
                int index3 = 0;
                string[] sentence3 = new string[length3];

                while (index3 != length3)
                {
                    sentence3[index3] = obj2CompleteDialogue[index3];
                    index3++;
                }

                System.sentenceArray.Add(sentence3);

                //DialogueHUD.SetActive(true);
                System.StartDialogue();

                while (System.isTyping)
                {
                    yield return null;
                }
                System.array++;
                break;           
        }
        //yield return new WaitForSeconds(5f);

        //DialogueHUD.SetActive(false);

        //State_Data.Instance._canBeHit = true;
        State_Data.Instance._hectorDialogue += 1;

        Loader.LoadLevel("Overworld");
        yield return new WaitForSeconds(0.5f);
        camObject.SetActive(false);
    }
}
