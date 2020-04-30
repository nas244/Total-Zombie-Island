using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_AI : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    Animator Panel;

    [SerializeField]
    GameObject PlayerObject;

    Player_movement PlayerMov;

    [SerializeField]
    int idle, talking, lookRadius, interactRadius, option;

    int conversation = 1;

    [SerializeField]
    string[] initialDialogue, regDialogue, wonDialogue, lostDialogue, notHighEnoughDialouge;

    [SerializeField]
    TextMeshProUGUI optionText1, optionText2;

    [SerializeField]
    string level, NPC_name, option1, option2;

    [SerializeField]
    GameObject OptionPanel, Icon, DialogueHUD;
 
    GameObject iconObject;

    public LevelLoader Loader;
    public Score_Controller Score;
    public DialogueSystem System;
    bool needsResponse = false;
    bool isLocked;
    bool canBePressed = true;
    public static bool pickingOption = false;

    [SerializeField]
    float requiredVal;

    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerMov = PlayerObject.GetComponent<Player_movement>();
        System = System.GetComponent<DialogueSystem>();
        Score = Score.GetComponent<Score_Controller>();

        iconObject = Instantiate(Icon, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
        iconObject.name = NPC_name + " icon";
        iconObject.SetActive(false);
        DialogueHUD.SetActive(false);
        OptionPanel.SetActive(false);

        anim.SetInteger("Animation_int", idle);
    }

    void Update()
    {
        TriggerRange();
        //Debug.Log("Conversation: " + conversation);
        //Debug.Log("Option: " + option);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    void TriggerRange()
    {
        // Gives enemy a range that will cause it to move once the player enters it
        if ((PlayerObject.transform.position - transform.position).sqrMagnitude < lookRadius * lookRadius)
        {
            //Debug.Log("within range");
            iconObject.SetActive(true);

            // Rotate the enemy too always face the player.
            Vector3 targetPosition = new Vector3(PlayerObject.transform.position.x, transform.position.y, PlayerObject.transform.position.z);
            transform.LookAt(targetPosition);
            iconObject.transform.LookAt(targetPosition);

            if ((PlayerObject.transform.position - transform.position).sqrMagnitude < interactRadius * interactRadius)
            {
                if (Input.GetKeyDown(KeyCode.E) && canBePressed)
                {
                    if (!System.isTyping)
                    {
                        if (Cursor.lockState == CursorLockMode.Locked) { isLocked = true; }

                        canBePressed = false;
                        // Start conversation
                        StartCoroutine(SetDialogue());
                        Debug.Log("start conversation");
                    }
                }
            }
        }

        else { iconObject.SetActive(false); }
    }

    IEnumerator SetDialogue()
    {
        Cursor.lockState = CursorLockMode.None;

        //Debug.Log(State_Data.Instance._score + "\t" + requiredVal);
        Debug.Log(Score._rating);
        if (Score._rating < requiredVal)
        {
            Debug.Log("Case 0");

            int length = notHighEnoughDialouge.Length;
            int index = 0;
            Debug.Log("Dialogue Length: " + length);
            string[] sentence = new string[length];

            while (index != length)
            {
                sentence[index] = notHighEnoughDialouge[index];
                index++;
            }

            System.sentenceArray.Add(sentence);

            DialogueHUD.SetActive(true);
            System.StartDialogue();

            while (System.isTyping)
            {
                yield return null;
            }
            System.array++;

            //yield break;
        }

        else
        {
            switch (conversation)
            {
                case 1:
                    Debug.Log("Case 1");
                    //update this so different dialogue if you have completed previous objective needed first          
                    needsResponse = true;
                    //System.sentenceArray[0] = initialDialogue;
                    //int index = 0;
                    //foreach (char letter in initialDialogue[letter].ToCharArray())

                    int length = initialDialogue.Length;
                    int index = 0;
                    Debug.Log("Dialogue Length: " + length);
                    string[] sentence = new string[length]; //{ initialDialogue[] };

                    while (index != length)
                    {
                        sentence[index] = initialDialogue[index];
                        index++;
                    }
                    //string[] sentence = initialDialogue;

                    //System.sentenceArray.Add(initialDialogue);
                    System.sentenceArray.Add(sentence);
                    //System.array++;

                    DialogueHUD.SetActive(true);
                    System.StartDialogue();
                    //System.array++;

                    //Cursor.lockState = CursorLockMode.None;

                    while (System.isTyping)
                    {
                        yield return null;
                    }
                    System.array++;

                    break;

                case 2:
                    Debug.Log("Case 2");
                    needsResponse = true;

                    int length2 = regDialogue.Length;
                    int index2 = 0;
                    Debug.Log("Dialogue Length: " + length2);
                    //string[] sentence2 = new string[] { regDialogue };
                    string[] sentence2 = new string[length2];

                    while (index2 != length2)
                    {
                        sentence2[index2] = regDialogue[index2];
                        index2++;
                    }
                    //System.sentenceArray[0] = regDialogue;
                    System.sentenceArray.Add(sentence2);
                    //System.array++;

                    DialogueHUD.SetActive(true);
                    System.StartDialogue();

                    while (System.isTyping)
                    {
                        yield return null;
                    }
                    System.array++;

                    break;

                case 3:
                    System.sentenceArray.Add(wonDialogue);

                    DialogueHUD.SetActive(true);
                    System.StartDialogue();

                    //Cursor.lockState = CursorLockMode.None;

                    while (System.isTyping)
                    {
                        yield return null;
                    }

                    break;

                case 4:
                    System.sentenceArray.Add(lostDialogue);

                    DialogueHUD.SetActive(true);
                    System.StartDialogue();

                    //Cursor.lockState = CursorLockMode.None;

                    while (System.isTyping)
                    {
                        yield return null;
                    }

                    break;
            }
        }
        

        DialogueHUD.SetActive(false);

        if (needsResponse)
        {
            pickingOption = true;
            SelectOption();

            //Time.timeScale = 0f;
            // waits until player chooses option before continuing
            //Debug.Log("Option = " + option);
            while (option == 0)
            {
                yield return null;
            }
            //Debug.Log("Picked");
            //Time.timeScale = 1f;

            Panel.SetTrigger("Panel");

            yield return new WaitForSeconds(0.5f);

            OptionPanel.SetActive(false);

            yield return new WaitForSeconds(1f);
        }

        pickingOption = false;
        NPC_Response();


        if (isLocked) { Cursor.lockState = CursorLockMode.Locked; }

        yield return new WaitForSeconds(1);
        canBePressed = true;
    }

    void SelectOption()
    {
        needsResponse = false;
        DialogueHUD.SetActive(false);
        OptionPanel.SetActive(true);

        optionText1.text = option1;
        optionText2.text = option2;
    }

    public void OnOptionButton(int choice)
    {
        //Debug.Log("Picked Option " + choice);
        //SoundManager.PlaySound(SoundManager.Sound.Beep);
        option = choice;
    }

    void NPC_Response()
    {
        // put enemy responses
        switch (conversation)
        {
            case 1:
                switch (option)
                {
                    case 1:
                        option = 0;

                        PlayerMov.Save_Data();

                        Debug.Log("Loading " + level);
                        Loader.LoadLevel(level);
                        break;

                    case 2:
                        option = 0;
                        conversation = 2;
                        //System.array++;
                        //UpdateConversation(2);
                        break;
                }
                break;

            case 2:
                switch (option)
                {
                    case 1:
                        option = 0;

                        PlayerMov.Save_Data();

                        Debug.Log("Loading " + level);
                        Loader.LoadLevel(level);
                        break;

                    case 2: 
                        option = 0;
                        //System.array++;
                        break;
                }
                break;
        }
    }
}
