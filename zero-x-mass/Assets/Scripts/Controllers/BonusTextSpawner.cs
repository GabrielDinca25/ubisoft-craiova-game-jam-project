using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTextSpawner : MonoBehaviour
{
    public void SpawnText(string name, Vector3 position)
    {
        foreach(Transform child in transform)
        {
            if(child.name == name)
            {
                child.transform.position = position;
                child.gameObject.SetActive(true);
            }
        }
    }
}
