using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScoreText : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.position += Vector3.up * Time.fixedDeltaTime;
    }
    private void OnEnable()
    {
        Invoke("DisableThis", 1f);
    }

    private void DisableThis()
    {
        gameObject.SetActive(false);
    }
}
