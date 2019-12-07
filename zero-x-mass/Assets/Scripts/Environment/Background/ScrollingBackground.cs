using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    private void Awake()
    {
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        transform.localScale = new Vector3(1 * Screen.height / 1080, 1 * Screen.width / 1920, 0);
    }
    void FixedUpdate()
    {
        transform.position -= transform.right * Time.fixedDeltaTime;
    }

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(44.4f, 0, 0);
    }
}
