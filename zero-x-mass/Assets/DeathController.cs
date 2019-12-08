using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [HideInInspector] public static DeathController instance;
    public AudioSource audioSource;
    public Camera cam;

    public void EndGame(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        cam.GetComponent<AudioSource>().Stop();
        audioSource.Play();
    }

    public void AniamtionEnded()
    {
        gameObject.SetActive(false);
        GameController.instance.GameOver();
    }
}
