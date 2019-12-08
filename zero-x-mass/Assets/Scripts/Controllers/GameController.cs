using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [HideInInspector] public static GameController instance;

    [Header("PauseSettings")]
    public bool gameOver = false;

    [Header("PlayerSettings")]
    public GameObject player;
    private GameObject playerDust;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Animator playerAnim;
    [HideInInspector] public Rigidbody2D playerRb2d;

    public bool gravity;
    public float currentScore;
    public int score;
    public int coins;

    public Camera cam;

    public BonusTextSpawner bonusTextSpawner;
    public AudioSource audioSource;
    public GameObject deathMenu;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text coinsText;

    public GameObject[] Players;
    public DeathController deathController;

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

        foreach(GameObject player in Players)
        {
            if(player.name == ("Player" + SettingsManager.instance.CharacterSelected))
            {
                GameObject.Instantiate(player, new Vector3(1,0,0), Quaternion.identity);
                return;
            }
        }
    }

    private void Start()
    {
        gravity         = false;
        player          = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        playerAnim      = player.GetComponent<Animator>();
        playerRb2d      = player.GetComponent<Rigidbody2D>();
        playerDust      = player.transform.GetChild(0).gameObject;
        score = 0;
        coins = SettingsManager.instance.Coins;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMenu();
        }

        currentScore += Time.deltaTime;
    }

    public void ChangeMovement()
    {
        audioSource.Play();
        cam.GetComponent<CameraShaker>().shakeDuration = 1f;
        Invoke("DoTheMovementChange", 1f);
     }

    public void DoTheMovementChange()
    { 
        gravity = !gravity;
        if (gravity)
        {
            playerRb2d.velocity = Vector2.zero;
            player.GetComponent<PlayerZeroGMovement>().moving  = false;
            player.GetComponent<PlayerZeroGMovement>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled      = true;

            playerDust.SetActive(false);
            playerAnim.SetBool("flying" , false);
            playerAnim.SetBool("walking", true);

            playerTransform.rotation = Quaternion.identity;

            player.GetComponentInChildren<BoxCollider2D>().enabled    = false;
            player.GetComponentInChildren<CircleCollider2D>().enabled = true;
            playerRb2d.isKinematic                             = false;
        }
        else
        {
            playerRb2d.velocity = Vector2.zero;
            player.GetComponentInChildren<PlayerMovement>().moving       = false;
            player.GetComponentInChildren<PlayerMovement>().enabled      = false;
            player.GetComponentInChildren<PlayerZeroGMovement>().enabled = true;
            playerAnim.SetBool("walking", false);
            playerAnim.SetBool("flying" , true);
            playerDust.SetActive(true);

            playerRb2d.velocity = new Vector2(0, 1f);

            player.GetComponentInChildren<CircleCollider2D>().enabled = false;
            player.GetComponentInChildren<BoxCollider2D>().enabled    = true;

            playerRb2d.isKinematic                                       = true;
        }
        ObjectsManager.instance.ChangeMovement();
    }



    public void AddBonus(string name, Vector3 position)
    {
        int bonus = Int32.Parse(name);
        coins    += bonus;

        bonusTextSpawner.SpawnText(name, position);
    }

    public void UpdateScore()
    {
        scoreText.text = score + "";
    }

    public void CallRevertGravity()
    {
        Invoke("RevertGravity", 5f);
    }

    public void RevertGravity()
    {
        ChangeMovement();
    }

    public void GameOver()
    {
        if (gameOver)
        {
            return;
        }
        gameOver = true;
        ShowDeathMenu();
        SettingsManager.instance.Save();
    }

    public void ShowDeathMenu()
    {
        score = (int)currentScore;
        if(score > SettingsManager.instance.Highscore)
        {
            SettingsManager.instance.Highscore = score;
        }
        scoreText.text     = score + "";
        highScoreText.text = SettingsManager.instance.Highscore + "";
        coinsText.text     = coins +"";
        SettingsManager.instance.Coins += coins;

        deathMenu.SetActive(true);
    }

    public void LoadMenu()
    {
        SettingsManager.instance.LoadMenu();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame(Vector3 position)
    {
        deathController.EndGame(position);
    }
}
