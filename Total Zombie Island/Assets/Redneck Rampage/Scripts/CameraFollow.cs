using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // init some vars editable in unity
    public GameObject objectToFollow;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        // calculate interpolation
        float interpolation = speed * Time.deltaTime;

        // calculate change in position
        Vector3 position = this.transform.position;
        position.z = Mathf.Lerp(
            this.transform.position.z,
            // objectToFollow.transform.position.z,
            Mathf.Clamp(objectToFollow.transform.position.z, 118, 196),  // prevent the player from moving too far left or right
            interpolation
        );

        // move the camera
        this.transform.position = position;
    }
}
