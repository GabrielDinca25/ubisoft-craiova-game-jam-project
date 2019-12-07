using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public string colliderType;
    public Vector2 spawnLimitsForY;

    void FixedUpdate()
    {
        transform.position -= transform.right * ObjectsManager.instance.obstacleSpeed * Time.fixedDeltaTime;

        //if(transform.position.x < -6)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    private void OnEnable()
    {
        float obstacleY = Random.Range(spawnLimitsForY.x, spawnLimitsForY.y);

        transform.position = new Vector3(ObjectsManager.instance.obstacleSpawnPositionX, obstacleY);

        GetComponent<Rigidbody2D>().angularVelocity = 400;
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        ObjectsManager.instance.SpawnObstacle();
    }

    public void SetGravity()
    {
        if(GameController.instance.gravity)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;

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
        }
        else
        {
            GetComponent<Rigidbody2D>().isKinematic = true;

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
        }
    }
}
