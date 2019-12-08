using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsMovement : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position -= Vector3.right * 2 * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        float obstacleY = Random.Range(-3, 3);

        transform.position = new Vector3(30, obstacleY);

        GetComponent<Rigidbody2D>().angularVelocity = 200f;
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

}
