using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SniperScope : MonoBehaviour
{
    CameraShaker Shake;
    GameHandler Handler;

    private Camera cam;

    [SerializeField]
    private float speedH = 2f, speedV = 2f, yaw = 0f, pitch = 0f;

    [SerializeField]
    private float targetZoom, zoomFactor = 3f, zoomLerpSpeed = 10;

    public bool canAim = true;
    public bool canShoot = true;

    public TextMeshProUGUI bulletCount;

    private void Start()
    {
        cam = Camera.main;

        targetZoom = cam.orthographicSize;

        Shake = cam.GetComponent<CameraShaker>();
        Handler = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameHandler>();

        bulletCount.text = "Bullets: " + Handler.numBullets.ToString();
    }

    void Update()
    {
        MoveCamera();
        ZoomCamera();

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        if (canShoot)
        {
            RaycastHit hit;

            SoundManager.PlaySound(SoundManager.Sound.Shot);
            Destroy(GameObject.Find("Sound"), 1);
            Shake.shouldShake = true;

            Handler.numBullets -= 1;
            Handler.bulletsUsed += 1;
            bulletCount.text = "Bullets: " + Handler.numBullets.ToString();

            // If you hit something
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                canShoot = false;

                if (hit.collider.transform.tag == "Enemy")
                {
                    Debug.Log("Hit the enemy!");
                    Handler.enemiesKIA += 1;
                    hit.transform.SendMessage("OnRaycastReceived");
                    Handler.score += 50 * Handler.currentStreak;
                    Handler.currentStreak += 0.1f;
                }
                else if (hit.collider.transform.tag == "Head")
                {
                    Debug.Log("You got a headshot!");
                    Handler.enemiesKIA += 1;
                    SoundManager.PlaySound(SoundManager.Sound.Headshot);
                    Destroy(GameObject.Find("Sound"), 1);
                    hit.transform.SendMessage("OnRaycastReceived");
                    Handler.score += 150 * Handler.currentStreak;
                    Handler.currentStreak += 0.1f;
                }
                else
                {
                    Handler.currentStreak = 1;
                    Debug.Log("You missed...");
                    Debug.Log(Handler.currentStreak);
                }

                SoundManager.PlaySound(SoundManager.Sound.Reload);
                Destroy(GameObject.Find("Sound"), 1);

                yield return new WaitForSeconds(1f);

                canShoot = true;
            }

            if (Handler.numBullets == 0)
            {
                canShoot = false;
            }
        }
    }

    void MoveCamera()
    {
        if (canAim)
        {
            // make the mouse stay center
            Cursor.lockState = CursorLockMode.Locked;

            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            // Limit how far you can move scope
            yaw = Mathf.Clamp(yaw, 50f, 120f);
            pitch = Mathf.Clamp(pitch, 7.5f, 35f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
    }

    void ZoomCamera()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        // Set zoom speed and limit how far you can zoom
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 2.5f, 10f);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
