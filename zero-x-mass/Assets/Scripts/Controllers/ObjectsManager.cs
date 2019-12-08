using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    [HideInInspector] public static ObjectsManager instance;

    public float obstacleSpawnPositionX = 30;
    public float obstacleSpeed = 5;
    public float obstacleRespawnPositionX = 30;
    public float obstacleRotationSpeed = 200f;
    public float enemyMovementSpeed = 5;

    [HideInInspector] public float lastobstacleSpeed;
    [HideInInspector] public float lastenemyMovementSpeed;
    public Vector3 spawnPosition;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine("StartSpawning");
    }

    IEnumerator StartSpawning()
    {
        while (!GameController.instance.gameOver)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(2f);
        }
    }

    public void ChangeMovement()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<ObjectMovement>().SetGravity();
        }
    }


    public void SpawnObstacle()
    {
        int nextObstacle = _GetNextObstacleNumber();
        if (nextObstacle != transform.childCount)
        {
            transform.GetChild(nextObstacle).position = spawnPosition;
            transform.GetChild(nextObstacle).gameObject.SetActive(true);
        }
        else
        {
            _InstantiateNewObstacle();
        }
    }

    private int _GetNextObstacleNumber()
    {
        int obstacleNumber = Random.Range(0, transform.childCount);

        if (!transform.GetChild(obstacleNumber).gameObject.activeSelf)
        {
            return obstacleNumber;
        }

        for (int i = obstacleNumber; i < transform.childCount; ++i)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                return i;
            }
        }
        for (int i = 0; i < obstacleNumber; ++i)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                return i;
            }
        }
        return transform.childCount;
    }

    private void _InstantiateNewObstacle()
    {
        Debug.Log("lol");
        int obstacleNumber = Random.Range(0, transform.childCount);

        GameObject newobstacle = Instantiate(
            transform.GetChild(obstacleNumber).gameObject,
            spawnPosition,
            Quaternion.identity
            );
        newobstacle.transform.parent = transform;

    }

    public void PauseEnvironment()
    {
        lastobstacleSpeed = obstacleSpeed;
        obstacleSpeed = 0;
        lastenemyMovementSpeed = enemyMovementSpeed;
        enemyMovementSpeed = 0;
    }

}
