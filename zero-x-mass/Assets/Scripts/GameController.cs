using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [HideInInspector] public static GameController instance;

    [Header("PauseSettings")]
    public bool gameOver = false;

    [Header("PlayerSettings")]
    public GameObject player;
    public Transform playerTransform;
    public float playerSpeed;
    public bool gravity;

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

    public void Update()
    {
        if (Input.GetMouseButton(1))
        {
            gravity = !gravity;
            ChangeMovement();
        }
    }

    private void Start()
    {
        gravity = false;
    }

    public void ChangeMovement()
    {
        if (gravity)
        {
            player.GetComponent<Rigidbody2D>().isKinematic     = false;
            player.GetComponent<PlayerZeroGMovement>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled      = true;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().isKinematic     = true;
            player.GetComponent<PlayerZeroGMovement>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled      = false;
        }
    }

}
