using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { START, PLAY, END }

public class GameHandler : MonoBehaviour
{
    AudioSource music;
    public static GameHandler instance;

    public int numBullets = 30;
    public int bulletsUsed;
    public int enemiesKIA;
    int difference;
    float bonus;
    public int countdownTime;
    public int startTime;
    public float score = 0;
    public float currentStreak = 1.0f;
    float finalScore;
    public float scoreTimer;

    public GameObject TimerHUD;
    public GameObject EndGameHUD;
    public GameObject FinalScoreHUD;
    public GameObject ScopeHUD;
    public GameObject StartTimerHUD;

    public TextMeshProUGUI countdownDisplay;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI startCountdownText;

    SniperScope Scope;

    public LevelLoader Loader;

    public BattleState state;

    string currentScene;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        //PlayerMov = PlayerObject.GetComponent<Player_movement>();

        instance = this;

        currentScene = SceneManager.GetActiveScene().name;

        StartCoroutine(SetupGame());
    }

    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    IEnumerator SetupGame()
    {
        EndGameHUD.SetActive(false);
        FinalScoreHUD.SetActive(false);
        StartTimerHUD.SetActive(false);

        Scope = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SniperScope>();
        Scope.canAim = false;
        Scope.canShoot = false;

        yield return new WaitForSeconds(2f);
        state = BattleState.PLAY;
        StartCoroutine(StartGame());       
    }

    public IEnumerator StartGame()
    {

        StartTimerHUD.SetActive(true);

        while (startTime > 0)
        {
            startCountdownText.text = startTime.ToString();
            SoundManager.PlaySound(SoundManager.Sound.Countdown);

            yield return new WaitForSeconds(1f);

            startTime--;
        }

        music.Play();
        SoundManager.PlaySound(SoundManager.Sound.Start);

        StartTimerHUD.SetActive(false);

        Scope.canAim = true;
        Scope.canShoot = true;

        while (countdownTime > 0)
        {
            countdownDisplay.text = "TIME LEFT: " + countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        state = BattleState.END;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        music.Stop();
        SoundManager.PlaySound(SoundManager.Sound.Finished);

        TimerHUD.SetActive(false);
        ScopeHUD.SetActive(false);
        EndGameHUD.SetActive(true);

        Scope.canAim = false;
        Scope.canShoot = false;
        Cursor.lockState = CursorLockMode.None;

        yield return new WaitForSeconds(3f);

        EndGameHUD.SetActive(false);

        yield return new WaitForSeconds(1f);

        FinalScoreHUD.SetActive(true);

        while (finalScore  <= score)
        {
            finalScoreText.text = "FINAL SCORE: " + finalScore;

            yield return new WaitForSeconds(scoreTimer);

            finalScore++;
        }

        bonus = GetBonus();

        yield return new WaitForSeconds(1f);

        finalScoreText.text = "FINAL SCORE: " + finalScore + " X BONUS";

        yield return new WaitForSeconds(1f);

        finalScore *= bonus;

        finalScoreText.text = "FINAL SCORE: " + (int)finalScore;

        if (finalScore >= 500)
        {
            SoundManager.PlaySound(SoundManager.Sound.Win);
            if (!State_Data.Instance._MG1Complete)
            {
                Debug.Log("Completed first time");
                State_Data.Instance._MG1Complete = true;
                State_Data.Instance._currentObjective += 1;
                State_Data.Instance._scoreCap += 1;
                State_Data.Instance._score += 1;
                State_Data.Instance._spawnLimit += 3;
                State_Data.Instance._spawnDelay -= 1;
                //yield break;
            }

            else if (State_Data.Instance._MG1Complete)
            {
                Debug.Log("Completed again");
                State_Data.Instance._score += 0.25f;
                //yield break;
            }
        }

        else
        {
            SoundManager.PlaySound(SoundManager.Sound.Fail);
        }

        yield return new WaitForSeconds(2f);

        Loader.LoadLevel("Overworld");
    }

    float GetBonus()
    {
        difference = bulletsUsed - enemiesKIA;

        if (enemiesKIA - difference < 0f)
            return 1f;

        else
            return 1f + ((enemiesKIA - difference) / 100f);
    }
}
