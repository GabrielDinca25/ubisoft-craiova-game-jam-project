using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScoreTextMovement : MonoBehaviour
{

    void FixedUpdate()
    {
        transform.position += transform.up * 2f * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        Invoke("DisableThis", 1f);
    }

    void DisableThis()
    {
        gameObject.SetActive(false);
    }
}
