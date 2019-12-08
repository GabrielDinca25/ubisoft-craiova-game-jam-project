using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    int min;
    int max;

    private void Start()
    {
        StartCoroutine("SpawnBonuses");
        min = 3;
        max = 4;
    }

    IEnumerator SpawnBonuses()
    {
        int count = 0;
        while (!GameController.instance.gameOver)
        {
            int random = Random.Range(min, max);
            yield return new WaitForSeconds(random);
            SpawnBonus();
            count++;
            if(count > 7)
            {
                ObjectsManager.instance.obstacleSpeed = 10;
                max = 2;
                min = 1;
            }
        }
    }

    public void SpawnBonus()
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

}
