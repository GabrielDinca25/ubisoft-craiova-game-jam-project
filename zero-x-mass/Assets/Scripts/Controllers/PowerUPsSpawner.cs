using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUPsSpawner : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        StartCoroutine("SpawnPowerUPs");
    }

    IEnumerator SpawnPowerUPs()
    {
        int random = Random.Range(5, 6);
        yield return new WaitForSeconds(random);

        while (!GameController.instance.gameOver)
        {
            random = Random.Range(10, 12);
            yield return new WaitForSeconds(random);
            SpawnPowerUPS();
        }

    }

    public void SpawnPowerUPS()
    {
        int nextObstacle = _GetNextObstacleNumber();
        if (nextObstacle != transform.childCount)
        {
            transform.GetChild(nextObstacle).gameObject.SetActive(true);
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

    public void PowerUp(string name)
    {
        switch (name)
        {
            case "powder":
                GameController.instance.ChangeMovement();
                GameController.instance.CallRevertGravity();
                break;
            case "sleigh":
                audioSource.Play();
                break;
        }

    }
}
