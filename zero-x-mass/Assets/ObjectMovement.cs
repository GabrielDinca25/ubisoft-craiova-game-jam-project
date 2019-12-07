using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public string colliderType;
    public Vector2 spawnLimitsForY;
    public float groundPosition;

    void FixedUpdate()
    {
        transform.position -= Vector3.right * ObjectsManager.instance.obstacleSpeed * Time.fixedDeltaTime;

        //if(transform.position.x < -6)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    private void OnEnable()
    {
        float obstacleY = Random.Range(spawnLimitsForY.x, spawnLimitsForY.y);

        transform.position = new Vector3(ObjectsManager.instance.obstacleSpawnPositionX, obstacleY);

        GetComponent<Rigidbody2D>().angularVelocity = ObjectsManager.instance.obstacleRotationSpeed;
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        ObjectsManager.instance.SpawnObstacle();
    }

    public void SetGravity()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (GameController.instance.gravity)
        {
            rb2d.isKinematic = false;

            switch (colliderType)
            {
                case "BoxCollider2D":
                    GetComponent<BoxCollider2D>().isTrigger    = false;
                    break;
                case "CircleCollider2D":
                    GetComponent<CircleCollider2D>().isTrigger = false;
                    break;
                case "EdgeCollider2D":
                    GetComponent<EdgeCollider2D>().isTrigger    = false;
                    break;
            }
            if(transform.position.y < groundPosition)
            {
                transform.position = new Vector2(transform.position.x, groundPosition);
            }
        }
        else
        {
            rb2d             = GetComponent<Rigidbody2D>();
            rb2d.isKinematic = true;

            switch (colliderType)
            {
                case "BoxCollider2D":
                    GetComponent<BoxCollider2D>().isTrigger    = true;
                    break;
                case "CircleCollider2D":
                    GetComponent<CircleCollider2D>().isTrigger = true;
                    break;
                case "EdgeCollider2D":
                    GetComponent<EdgeCollider2D>().isTrigger    = true;
                    break;
            }

            rb2d.velocity = new Vector2(Random.Range(-1,1), Random.Range(0, 1f));
        }
    }
}
