using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AI : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    GameObject PlayerObject;

    Player_movement PlayerMov;

    [SerializeField]
    int idle, talking, lookRadius, interactRadius, conversation = 1;

    [SerializeField]
    string[] initialDialogue, wonDialogue, lostDialogue;

    public string level;

    public GameObject Icon;
    public GameObject DialogueHUD;
    GameObject iconObject;

    public LevelLoader Loader;

    Transform target;

    DialogueSystem System;

    void Start()
    {
        anim = GetComponent<Animator>();
        System = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueSystem>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        iconObject = Instantiate(Icon, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
        iconObject.SetActive(false);
        DialogueHUD.SetActive(false);

        anim.SetInteger("Animation_int", idle);
    }

    private void Update()
    {
        TriggerRange();
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
        float distance = Vector3.Distance(target.position, transform.position);

        // Gives enemy a range that will cause it to move once the player enters it
        if (distance <= lookRadius)
        {
            iconObject.SetActive(true);

            // Rotate the enemy too always face the player. Locks every axis except Y so that it only rotates on Y axis. Change this for 3D sections
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(targetPosition);
            iconObject.transform.LookAt(targetPosition);

            if (distance <= interactRadius)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!System.isTyping)
                    {
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
        switch (conversation)
        {
            case 1:
                System.sentenceArray.Add(initialDialogue);

                DialogueHUD.SetActive(true);
                System.StartDialogue();

                Cursor.lockState = CursorLockMode.None;

                while (System.isTyping)
                {
                    yield return null;
                }

                break;

            case 2:
                System.sentenceArray.Add(wonDialogue);

                DialogueHUD.SetActive(true);
                System.StartDialogue();

                Cursor.lockState = CursorLockMode.None;

                while (System.isTyping)
                {
                    yield return null;
                }

                break;

            case 3:
                System.sentenceArray.Add(lostDialogue);

                DialogueHUD.SetActive(true);
                System.StartDialogue();

                Cursor.lockState = CursorLockMode.None;

                while (System.isTyping)
                {
                    yield return null;
                }

                break;
        }

        DialogueHUD.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        PlayerMov.Save_Data();

        Loader.LoadLevel(level);
    }

}
