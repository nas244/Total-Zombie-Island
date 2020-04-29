using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Countdown : MonoBehaviour
{
    
    public float timestart = 10;
    public Text countdown;
    public GameObject Explosion;
    public GameObject Character;
    public GameObject Smoke;
    public Animator GameOver;
    public GameObject bombmanager;
    public AudioSource bomb;
    public float Wait;

    // Start is called before the first frame update
    void Start()
    {
        countdown.text = timestart.ToString();

    }

        // Update is called once per frame
    void Update()
    {

        timestart -= Time.deltaTime;
        countdown.text = Mathf.Round(timestart).ToString();


        if (timestart <= 0)
        {
            timestart = 0;
            Explosion.gameObject.SetActive(true);
            Smoke.gameObject.SetActive(true);
            Character.gameObject.SetActive(false);
            GameOver.SetTrigger("start");
    

        }
        else
        {
            Explosion.gameObject.SetActive(false);
        }
    }
    
    public void BackToOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }

}
