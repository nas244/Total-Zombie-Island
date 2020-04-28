using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField]
    private float power = 0.7f, duration = 1f, slowDownAmount = 1f;

    public bool shouldShake = false;

    Transform camera;

    Vector3 startPosition;
    float initialDuration;

    void Start()
    {
        camera = Camera.main.transform;
        startPosition = camera.localPosition;
        initialDuration = duration;
    }

    void Update()
    {
        if (shouldShake)
        {
            if (duration > 0)
            {
                camera.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }

            else
            {
                shouldShake = false;
                duration = initialDuration;
                camera.localPosition = startPosition;
            }
        }
    }
}
