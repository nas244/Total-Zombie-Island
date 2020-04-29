using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SplashIntro : MonoBehaviour
{
    [SerializeField]
    GameObject[] Team, IbnSina;

    public GameObject Logo;

    // Start is called before the first frame update
    void Start()
    {

        Logo.SetActive(false);

        foreach (GameObject letter in Team)
        {
            letter.SetActive(false);
        }

        foreach (GameObject letter in IbnSina)
        {
            letter.SetActive(false);
        }


        StartCoroutine(ShowLetters());
    }

    IEnumerator ShowLetters()
    {

        foreach (GameObject letter in Team)
        {
            letter.SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }

        foreach (GameObject letter in IbnSina)
        {
            letter.SetActive(true);
            yield return new WaitForSeconds(0.35f);
        }

        foreach (GameObject letter in Team)
        {
            letter.GetComponent<Animator>().SetTrigger("Swell");
        }

        foreach (GameObject letter in IbnSina)
        {
            letter.GetComponent<Animator>().SetTrigger("Swell");
        }

        yield return new WaitForSeconds(2);

        Logo.SetActive(true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Start");
    }


}
