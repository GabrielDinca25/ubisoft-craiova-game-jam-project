using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    void FixedUpdate()
    {
        transform.position -= transform.right * Time.fixedDeltaTime;
    }

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(25.2f, 0, 0);
    }
}
