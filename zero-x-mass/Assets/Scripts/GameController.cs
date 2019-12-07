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
    public GameObject playerDust;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Animator playerAnim;
    [HideInInspector] public Rigidbody2D playerRb2d;
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

    private void Start()
    {
        gravity         = false;
        playerTransform = player.GetComponent<Transform>();
        playerAnim      = player.GetComponent<Animator>();
        playerRb2d      = player.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gravity = !gravity;
            ChangeMovement();
        }
    }

    public void ChangeMovement()
    {
        if (gravity)
        {
            playerRb2d.isKinematic                             = false;
            player.GetComponent<PlayerZeroGMovement>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled      = true;

            playerDust.SetActive(false);
            playerAnim.SetBool("flying" , false);
            playerAnim.SetBool("walking", true);

            playerRb2d.velocity = new Vector2(0, -1.8f);
            playerTransform.rotation = Quaternion.identity;

            player.GetComponent<BoxCollider2D>().enabled    = false;
            player.GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            playerRb2d.isKinematic     = true;
            player.GetComponent<PlayerZeroGMovement>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled      = false;
            playerAnim.SetBool("walking", false);
            playerAnim.SetBool("flying" , true);
            playerDust.SetActive(true);

            playerRb2d.velocity = new Vector2(0, 1f);

            player.GetComponent<CircleCollider2D>().enabled = false;
            player.GetComponent<BoxCollider2D>().enabled    = true;
        }
    }

}
