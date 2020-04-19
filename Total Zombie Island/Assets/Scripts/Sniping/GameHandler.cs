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

    Unit playerUnit;
    Unit enemyUnit;

    BattleHUDBattle playerHUD;
    SniperScope Scope;

    public BattleState state;

    string currentScene;

    private void Start()
    {
        music = GetComponent<AudioSource>();

        instance = this;

        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Minigame 1")
            StartCoroutine(SetupGame(1));

        else if (currentScene == "Cityscape")
            state = BattleState.START;
            StartCoroutine(SetupGame(2));

    }

    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    IEnumerator SetupGame(int minigame)
    {
        switch (minigame)
        {
            case 1:
                //cameraFollow.Setup(() => playerTransform.position);
                playerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
                enemyUnit = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Unit>();
                playerHUD = GameObject.FindGameObjectWithTag("Phealth").GetComponent<BattleHUDBattle>();

                playerUnit.currentHP = playerUnit.maxHP;
                playerHUD.setHUD(playerUnit);
                break;

            case 2:
                EndGameHUD.SetActive(false);
                FinalScoreHUD.SetActive(false);
                StartTimerHUD.SetActive(false);

                Scope = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SniperScope>();
                Scope.canAim = false;
                Scope.canShoot = false;

                yield return new WaitForSeconds(2f);
                state = BattleState.PLAY;
                StartCoroutine(StartGame(2));
                break;
        }
    }

    IEnumerator StartGame(int game)
    {

        StartTimerHUD.SetActive(true);

        while (startTime > 0)
        {
            startCountdownText.text = startTime.ToString();
            SoundManager.PlaySound(SoundManager.Sound.Countdown);

            yield return new WaitForSeconds(1f);

            Destroy(GameObject.Find("Sound"));

            startTime--;
        }

        music.Play();
        SoundManager.PlaySound(SoundManager.Sound.Start);
        Destroy(GameObject.Find("Sound"), 1);

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
        Destroy(GameObject.Find("Sound"), 2);

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
            //scoreTimer -= 0.0009f;

        }

        bonus = GetBonus();
        Debug.Log("Bonus: " + bonus);

        yield return new WaitForSeconds(2f);

        finalScoreText.text = "FINAL SCORE: " + finalScore + " X BONUS";

        yield return new WaitForSeconds(2f);

        finalScore *= bonus;

        finalScoreText.text = "FINAL SCORE: " + (int)finalScore;

        if (finalScore >= 2000)
        {
            SoundManager.PlaySound(SoundManager.Sound.Win);
            Destroy(GameObject.Find("Sound"), 3);
        }

        else
        {
            SoundManager.PlaySound(SoundManager.Sound.Fail);
            Destroy(GameObject.Find("Sound"), 2);
        }

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Overworld");
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
