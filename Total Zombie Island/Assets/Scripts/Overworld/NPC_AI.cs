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
    int idle, talking, lookRadius, interactRadius, option, conversation = 1;

    [SerializeField]
    string[] initialDialogue, regDialogue, wonDialogue, lostDialogue;

    [SerializeField]
    TextMeshProUGUI optionText1, optionText2;

    [SerializeField]
    string level, NPC_name, option1, option2;

    [SerializeField]
    GameObject OptionPanel, Icon, DialogueHUD;
 
    GameObject iconObject;

    public LevelLoader Loader;

    public DialogueSystem System;
    bool needsResponse = false;
    bool isLocked;
    bool repeating;
    public static bool pickingOption = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerMov = PlayerObject.GetComponent<Player_movement>();
        System = System.GetComponent<DialogueSystem>();

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
        Debug.Log("Conversation: " + conversation);
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!System.isTyping)
                    {
                        if (Cursor.lockState == CursorLockMode.Locked) { isLocked = true; }

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

        repeating = false;

        switch (conversation)
        {
            case 1:
                //update this so different dialogue if you have completed previous objective needed first
                //conversation = 2;               
                needsResponse = true;
                System.sentenceArray.Add(initialDialogue);

                DialogueHUD.SetActive(true);
                System.StartDialogue();

                //Cursor.lockState = CursorLockMode.None;

                while (System.isTyping)
                {
                    yield return null;
                }

                break;

            case 2:
                needsResponse = true;
                System.sentenceArray.Add(regDialogue);

                DialogueHUD.SetActive(true);
                System.StartDialogue();

                //Cursor.lockState = CursorLockMode.None;

                while (System.isTyping)
                {
                    yield return null;
                }

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

        DialogueHUD.SetActive(false);

        if (needsResponse)
        {
            pickingOption = true;
            SelectOption();

            //Time.timeScale = 0f;
            // waits until player chooses option before continuing
            while (option == 0)
            {
                yield return null;
            }
            Debug.Log("option: " + option);
            //Time.timeScale = 1f;

            Panel.SetTrigger("Panel");

            yield return new WaitForSeconds(0.5f);

            OptionPanel.SetActive(false);

            yield return new WaitForSeconds(1f);
        }

        pickingOption = false;
        NPC_Response();

        if (repeating) { yield break; }
        else
        {
            if (isLocked) { Cursor.lockState = CursorLockMode.Locked; }
        }

        //repeating = false;
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
        //SoundManager.PlaySound(SoundManager.Sound.Beep);
        option = choice;
    }

    void UpdateConversation(int num) { conversation = num; }

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

                        Loader.LoadLevel(level);
                        break;

                    case 2:
                        option = 0;
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

                        Loader.LoadLevel(level);
                        break;

                    case 2: 
                        option = 0;
                        break;
                }
                break;
        }
    }
}
