using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleStates { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sadMusic;
    AudioSource victoryMusic1;
    AudioSource victoryMusic2;

    public static BattleSystem instance;
    public bool startPlaying;

    public Image blackFade;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject BattlePanel;
    public GameObject OptionPanel;
    public GameObject fadeObject;
    public GameObject victoryObject1;
    public GameObject victoryObject2;
    public GameObject attackButton;
    public GameObject defendButton;

    public Transform playerStation;
    public Transform enemyStation;

    Unit playerUnit;
    Unit enemyUnit;

    CharacterAnim Player;
    CharacterAnim Boss;

    public Animator Panel;
    public Animator Talk;

    DialogueSystem System;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI optionText1;
    public TextMeshProUGUI optionText2;
    public TextMeshProUGUI optionText3;
    public TextMeshProUGUI fadeDisplay;

    public BattleHUDBattle enemyHUD;
    public BattleHUDBattle playerHUD;

    public BattleStates state;

    string[] sentence;
    bool Special;
    bool wonViaDialogue = false;

    bool needsResponse = false; 
    int timesTalked;
    int option; // stores the option you choose
    int reaction; // determines what the enemy will do in response to option picked
    int picked; // based off what option you pick, stores a value

    public LevelLoader Loader;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        music = GetComponent<AudioSource>();
        sadMusic = fadeObject.GetComponent<AudioSource>();
        victoryMusic1 = victoryObject1.GetComponent<AudioSource>();
        victoryMusic2 = victoryObject2.GetComponent<AudioSource>();

        instance = this;

        state = BattleStates.START;
        BattlePanel.SetActive(false);
        OptionPanel.SetActive(false);
        fadeObject.SetActive(false);
        fadeDisplay.canvasRenderer.SetAlpha(0.0f);
        blackFade.canvasRenderer.SetAlpha(0.0f);

        StartCoroutine(SetupBattle());
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAnim>();
        Boss = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CharacterAnim>();
        System = GetComponent<DialogueSystem>();
    }

    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    /*public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }*/

    IEnumerator SetupBattle()
    {
        // Spawn player
        GameObject playerGO = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGO.GetComponent<Unit>();

        // Spawn enemy
        GameObject enemyGO = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGO.GetComponent<Unit>();      

        enemyHUD.setHUD(enemyUnit);
        playerHUD.setHUD(playerUnit);

        yield return new WaitForSeconds(1f);

        string[] sentence = new string[]
        {
            "Dave: It's nothing personal kid. ",
            "Dave: They said i would be a free man if I beat you.",
            "Dave: You understand right? I mean, aren't you trapped here too?",
            "Dave: Eh whatever. I just wanna get the hell outta here."
        };

        System.sentenceArray.Add(sentence);
        System.StartDialogue();

        while (System.isTyping)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        state = BattleStates.PLAYERTURN;
        PlayerTurn();
    }


    IEnumerator PlayerAttack()
    {
        Player.isAttacking = true;

        yield return new WaitForSeconds(1f);

        Boss.isDamaged = true;
        SoundManager.PlaySound(SoundManager.Sound.Hurt);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "The attack is a success! Good job bro!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            yield return new WaitForSeconds(1);

            Boss.isDeath = true;
            Player.hasWon = true;
            state = BattleStates.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            yield return new WaitForSeconds(1);

            state = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }      
    }

    IEnumerator PlayerDefend()
    {
        Player.isDefending = true;

        playerUnit.defense = 0.75f;
        dialogueText.text = "You hear boss music so you decide to defend.";

        yield return new WaitForSeconds(2f);

        state = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerTalk()
    {
        Player.isTalking = true;

        option = 0;

        switch (timesTalked)
        {
            case 0:
                sentence = new string[] 
                {
                    "Dave: Why are you talking to me?",
                    "Dave: Shut up and fight me like a man"
                };

                System.sentenceArray.Add(sentence);
                System.array++;
                timesTalked++;
                break;

            case 1:
                sentence = new string[]
                {
                    "Dave: Still tryna talk, eh?",
                    "Dave: Fine, I'll bite", "Dave: What's your game here?"
                };

                needsResponse = true;
                System.sentenceArray.Add(sentence);
                System.array++;
                break;

            case 2:
                sentence = new string[]
                {
                    "Dave: You really wanna know about my past?",
                    "Dave: . . .",
                    "Dave: Tell me something.",
                    "Dave: If you saw someone you cared for getting hurt, what would you do?"
                };

                needsResponse = true;
                System.sentenceArray.Add(sentence);
                System.array++;
                break;

            case 3:
                sentence = new string[]
                {
                    "Dave: . . .",
                    "Dave: It's your lucky day kid.",
                    "Dave: You'll be the first person I tell.",
                    "Dave: The first person to hear the tale of Dave the monster.",
                    // fade in black screen
                };

                System.sentenceArray.Add(sentence);
                System.array++;
                break;

            case 4:
                sentence = new string[]
                {
                    "Dave: What the hell am I doing?",
                    "Dave: I'm doing the same thing that got me into this situation in the first place",
                    "Dave: You wanna get off this island right?",
                    "Dave: Well I won't try to stop you anymore",
                    "Dave: I'm done being a screw up . . .",
                    "Dave: . . .",
                    "Dave: Hey kid . . .",
                    "Dave: Thanks . . ."
                };

                System.sentenceArray.Add(sentence);
                System.array++;
                reaction = 5;
                break;
        }
       
        System.StartDialogue();

        while (System.isTyping)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // check if dialogue prompts response
        if (needsResponse)
        {
            SelectOption();

            // waits until player chooses option before continuing
            while (option == 0)
            {
                yield return null;
            }

            Panel.SetTrigger("Panel");

            yield return new WaitForSeconds(0.5f);

            OptionPanel.SetActive(false);

            yield return new WaitForSeconds(1f);
        }     

        state = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        Player.doneTalking = true;

        if (timesTalked == 3 && picked == 3) { Boss.isTalking = true;  } // This is so that it doesn't start the scene unless you talk to him again

        // Determine what kind of response the enemy should have
        bool respond = EnemyResponse();

        if (respond)
        {           
            System.StartDialogue();
        }

        while (System.isTyping)
        {
            yield return null;
        }

        fadeOut();
        if (sadMusic.isPlaying) { SoundManager.FadeAudio(sadMusic, 3f, 0f); yield return new WaitForSeconds(3f); }

        yield return new WaitForSeconds(0.5f);

        fadeObject.SetActive(false);

        switch (reaction)
        {
            // Determine what the AI will do based on response to dialogue
            case 1:
                // Attack
                reaction = 0;
                dialogueText.text = enemyUnit.unitName + " makes a move!";
                break;

            case 2:
                reaction = 0;
                dialogueText.text = enemyUnit.unitName + " is thinking about his past . . .";
                yield return new WaitForSeconds(2);
                break;

            case 3:
                reaction = 0;
                dialogueText.text = enemyUnit.unitName + " is lost in thought . . .";
                yield return new WaitForSeconds(2);

                Boss.misc = true;
                Player.doneDefending = true;
                Player.doneTalking = true;
                state = BattleStates.PLAYERTURN;
                PlayerTurn();
                yield break;

            case 4:
                reaction = 0;
                yield return new WaitForSeconds(2);
                Boss.isCrying = true;
                dialogueText.text = enemyUnit.unitName + " appears to be crying . . .";
                yield return new WaitForSeconds(3);

                Player.doneDefending = true;
                Player.doneTalking = true;
                state = BattleStates.PLAYERTURN;
                PlayerTurn();
                yield break;

            case 5:
                reaction = 0;
                wonViaDialogue = true;
                Boss.isDeath2 = true;
                Player.hasWon2 = true;
                state = BattleStates.WON;
                StartCoroutine(EndBattle());
                yield break;


            default:
                // Attack
                dialogueText.text = enemyUnit.unitName + " makes a move!";
                break;

        }     

        yield return new WaitForSeconds(1);
        Boss.isAttacking = true;
        yield return new WaitForSeconds(1);

        SoundManager.PlaySound(SoundManager.Sound.Hurt);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage * playerUnit.defense);
        Player.isDamaged = true;
        enemyHUD.SetHP(enemyUnit.currentHP);
        playerHUD.SetHP(playerUnit.currentHP);

        if (isDead)
        {
            yield return new WaitForSeconds(2);

            Player.doneDefending = true;
            Player.doneTalking = true;
            Player.isDeath = true;
            Boss.hasWon = true;

            state = BattleStates.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            yield return new WaitForSeconds(2);

            Player.doneDefending = true;
            Player.doneTalking = true;
         
            state = BattleStates.PLAYERTURN;
            PlayerTurn();
        }    
    }



    bool EnemyResponse()
    {
        // put enemy responses
        switch (timesTalked)
        {
            case 1:
                switch (option)
                {
                    case 1:
                        option = 0;
                        sentence = new string[]
                        {
                            "Dave: What? You some kinda hippy?",
                            "Dave: IDK why but that really grind my gears!",
                            "Dave: Fight back or die!!!"
                        };

                        System.sentenceArray.Add(sentence);
                        System.array++;
                        reaction = 1;
                        return true;

                    case 2:
                        option = 0;
                        sentence = new string[] 
                        {
                            "Dave: Why the hell would I do that?",
                            "Dave: If i kick your ass, I get a free ticket outta this place.",
                            "Dave: Unless you got a better deal than that, put up your fists and fight!"
                        };

                        System.sentenceArray.Add(sentence);
                        System.array++;
                        reaction = 1;
                        return true;

                    case 3:
                        option = 0;
                        sentence = new string[]
                        {
                            "Dave: You curious, eh?",
                            "Dave: Well sorry but I'm not telling my business to some stranger.",
                            "Dave: You understand, right?"
                        };
                        timesTalked++;

                        System.sentenceArray.Add(sentence);
                        System.array++;
                        reaction = 2;
                        return true;

                    default:
                        return false;
                }
            case 2:
                switch (option)
                {
                    case 1:
                        option = 0;
                        sentence = new string[]
                        {
                            "Dave: Is that right?",
                            "Dave: You got guts kid . . .",
                            "Dave: . . . "
                        };
                        timesTalked++;

                        System.sentenceArray.Add(sentence);
                        System.array++;
                        reaction = 3;
                        return true;

                    case 2:
                        option = 0;
                        sentence = new string[]
                        {
                            "Dave: So you would risk them getting hurt even more?",
                            "Dave: You can't rely on the cops to help in this sort of situation",
                            "Dave: By the time anyone can help you, they might be DEAD!",
                            "Dave: Sometimes you gotta take actions into your own hands",
                            "Dave: You just don't get it."
                        };

                        System.sentenceArray.Add(sentence);
                        System.array++;
                        reaction = 1;
                        return true;

                    case 3:
                        option = 0;
                        sentence = new string[]
                        {
                            "Dave: Hmph . . .",
                            "Dave: No reason",
                            "Dave: Forget what I said, just focus on fighting me."
                        };

                        System.sentenceArray.Add(sentence);
                        System.array++;
                        reaction = 1;
                        return true;

                    default:
                        return false;
                }

            case 3:
                if (Boss.isTalking)
                {
                    // do nothing
                }
                else
                {
                    return false;
                }

                fadeIn();
                sentence = new string[]
                {
                    "Dave: It's been about 2 years now.",
                    "Dave: My daughter had just turned 17 the other day.",
                    "Dave: I let her go to a party one of her friends was having that night.",
                    "Dave: I knew the host's parents, so I wasn't too worried.",
                    "Dave: It was about 2 hours after I dropped her off that I got a text",
                    "Dave: It read: 'halp'.",
                    "Dave: Me being the idiot that I was, didn't realize what she was trying to say.",
                    "Dave: I texted back: 'What do you mean? Are you drunk?'",
                    "Dave: A few minutes later, I got another text.",
                    "Dave: It read: 'HELP ME'.",
                    "Dave: It didn't take an engineer to realize what was going on.",
                    "Dave: I leaped outta the house and headed straight towards her location.",
                    "Dave: Thankfully, she agreed to have a tracker on her phone, so I knew exactly where she was.",
                    "Dave: I arrived at some abandoned house a couple blocks from the party",
                    "Dave: It didn't take long to figure out where my daughter was",
                    "Dave: I could hear faint whimpers coming from behind the house",
                    "Dave: I swiftly made my way towards the sound.",
                    "Dave: It was dark, but the moonlight was enough for me to see the horrible sight before me...",
                    "Dave: I could see my daughter collapsed on the ground.",
                    "Dave: She was in a pool of blood.",
                    "Dave: I could see stab wounds all over her body and a hole where her right eye should've been.",
                    "Dave: It didn't take long to find the culprit.",
                    "Dave: It was the party host.",
                    "Dave: Apparently he had a crush on my daughter, and confessed to her at the party.",
                    "Dave: When she rejected him, he decided to drug her drink.",
                    "Dave: My daughter must have fought back pretty aggressively as he had visible scratches and blood across his face.",
                    "Dave: . . .",
                    "Dave: My mind went blank.",
                    "Dave: No, it didn't go blank . . . ",
                    "Dave: It was filled with pure rage.",
                    "Dave: I tackled him. He went down quickly.",
                    "Dave: I started punching. . .",
                    "Dave: and punching . . .",
                    "Dave: and punching . . . . . .",
                    "Dave: Before I knew it, I could here sirens fast approaching.",
                    "Dave: My daughter must have been able to contact the cops at some point.",
                    "Dave: You can probably guess what happened next.",
                    "Dave: . . .",
                    "Dave: . . . . . .",
                    "Dave's voice begins to shake",
                    "Dave: She died in the hospital.",
                    "Dave: . . .",
                    "Dave: The kid, that bastard, was smart.",
                    "Dave: He must have worn gloves when he assaulted her, because there where no fingerprints or DNA anywhere.",
                    "Dave: At the trial, he claimed that I assaulted him and my daughter in a fit of drunken rage",
                    "Dave: Of course I had to have been drinking that night.",
                    "Dave: It was perfect. No victim to share their side, only him... and me.",
                    "Dave: And who do you think the judge believed? An old alcoholic or a promising young student with a bright future ahead of him?",
                    "Dave: Tch . . .",
                    "Dave: You know what the headlines read the day after my sentencing?",
                    "Dave: Dave the Monster: Justice prevails again",
                    "Dave: Our glorious justice system, blind and true, right?",
                    "Dave: What a joke",
                    "Dave: . . .",
                    "Dave: I'm done. I don't wanna talk anymore."
                };

                System.sentenceArray.Add(sentence);
                System.array++;
                timesTalked++;
                System.specialFade = true;
                reaction = 4;


                //StartCoroutine(StartFade(music, 3f, 0f));
                SoundManager.FadeAudio(music, 3f, 0f);
                sadMusic.Play();

                return true;

            default:
                return false;
        }
    }

    void SelectOption()
    {
        needsResponse = false;
        BattlePanel.SetActive(false);
        OptionPanel.SetActive(true);

        switch (timesTalked)
        {
            // give options based on how much you have talked successfully
            case 1:
                optionText1.text = "I'm a hugger not a fighter.";
                optionText2.text = "Help me get out of here.";
                optionText3.text = "What are you serving time for?";
                break;

            case 2:
                optionText1.text = "I would stop the one responsible";
                optionText2.text = "I would go get help";
                optionText3.text = "Why are you asking?";
                break;
        }
    }

    void PlayerTurn()
    {
        BattlePanel.SetActive(true);
        playerUnit.defense = 1;
        picked = 0;
        if (timesTalked == 4) { attackButton.SetActive(false); defendButton.SetActive(false); Talk.SetTrigger("Glowing"); }
        if (System.specialFade) { dialogueText.text = ". . ."; }
        else { dialogueText.text = "Time to bust a move! (that means do something!!!!!!!)"; }
    }

    public void OnAttackButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.Beep);
        if (state != BattleStates.PLAYERTURN)
            return;

        if (System.specialFade) { System.specialFade = false;  }
        BattlePanel.SetActive(false);
        picked = 1;
        StartCoroutine(PlayerAttack());
    }

    public void OnDefendButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.Beep);
        if (state != BattleStates.PLAYERTURN)
            return;

        if (System.specialFade) { System.specialFade = false; }
        BattlePanel.SetActive(false);
        picked = 2;
        StartCoroutine(PlayerDefend());
    }

    public void OnTalkButton()
    {
        SoundManager.PlaySound(SoundManager.Sound.Beep);
        if (state != BattleStates.PLAYERTURN)
            return;

        if (System.specialFade) { System.specialFade = false; }
        BattlePanel.SetActive(false);
        picked = 3;
        StartCoroutine(PlayerTalk());
    }

    public void OnOptionButton(int choice)
    {
        SoundManager.PlaySound(SoundManager.Sound.Beep);
        option = choice;
    }

    void fadeIn()
    {
        BattlePanel.SetActive(false);
        fadeObject.SetActive(true);
        fadeDisplay.CrossFadeAlpha(1, 2, false);
        blackFade.CrossFadeAlpha(1, 2, false);
    }

    void fadeOut()
    {
        fadeDisplay.CrossFadeAlpha(1, 2, false);
        blackFade.CrossFadeAlpha(0, 2, false);
    }

    IEnumerator EndBattle()
    {
        if (state == BattleStates.WON)
        {
            if (wonViaDialogue)
            {
                victoryMusic2.Play();
                dialogueText.text = "You convinced Dave to stop fighting!";
                // add something to trigger different end sequence
                // also make it so you can't do the battle again
            }

            else
            {
                victoryMusic1.Play();
                dialogueText.text = "WOW! You actually won. I always believed in you bro!";
                // add something to trigger different end sequence
            }

            if (!State_Data.Instance._MG3Complete)
            {
                Debug.Log("Completed first time");
                State_Data.Instance._MG3Complete = true;
                State_Data.Instance._currentObjective += 1;
                //State_Data.Instance._scoreCap += 1;
                State_Data.Instance._score += 1;
            }

            else if (State_Data.Instance._MG3Complete) // get rid of this because you shouldn't be able to battle again if you win
            {
                Debug.Log("Completed again");
                State_Data.Instance._score += 0.25f;
            }

        } else if (state == BattleStates.LOST)
        {
            dialogueText.text = "Aww you're dead bro. Hate to see it...";
        }

        yield return new WaitForSeconds(5);

        Loader.LoadLevel("Overworld");
    }
}
