using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI fadeDisplay;
    public List<string[]> sentenceArray = new List<string[]>();

    public int index;
    public int array;

    public float typingSpeed;
    public bool isTyping;
    public bool specialFade = false;
    bool oneTime = true; // this is to make the delay happen once

    public GameObject continueButton;
    public GameObject continueButtonFade;
    //public Animator textDisplayAnim;
    private AudioSource source;

    void Start()
    {
        source = textDisplay.GetComponent<AudioSource>();
        continueButton.SetActive(false);
        continueButtonFade.SetActive(false);
    }

    IEnumerator Type()
    {
        State_Data.Instance._canBeHit = false;
        Cursor.lockState = CursorLockMode.None;

        textDisplay.text = "";
        isTyping = true;

        // delay the text displaying for the special dialogue
        if (specialFade && oneTime) { yield return new WaitForSeconds(3); oneTime = false; }

        //Debug.Log("Array: " + array + "\t Index: " + index);
        foreach (char letter in sentenceArray[array][index].ToCharArray())
        {
            if (specialFade)
            {
                fadeDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            else
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        if (specialFade)
        {
            continueButtonFade.SetActive(true);
        }

        else { continueButton.SetActive(true); }
    }

    public void StartDialogue()
    {
        StartCoroutine(Type());
    }

    public void NextSentence()
    {
        //Debug.Log("Next Sentence");
        source.Play();
        continueButton.SetActive(false);

        if (index < sentenceArray[array].Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }

        else
        {
            isTyping = false;
            index = 0;

            textDisplay.text = "";
            continueButton.SetActive(false);
            State_Data.Instance._canBeHit = true;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void NextSentenceFade()
    {
        source.Play();
        continueButtonFade.SetActive(false);

        if (index < sentenceArray[array].Length - 1)
        {
            index++;
            fadeDisplay.text = "";
            StartCoroutine(Type());
        }

        else
        {
            isTyping = false;
            index = 0;

            fadeDisplay.text = "";
            continueButtonFade.SetActive(false);
        }
    }
}
