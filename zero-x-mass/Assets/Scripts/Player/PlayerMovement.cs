using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public bool moving;
    public bool jump;
    public bool tap;
    public float playerSpeed = 2000f;

    public Vector3 _movementStartPosition;
    public Vector3 _movementCurrentPosition;
    public Vector3 _movementLastPosition;
    public Vector3 _lastdirection;

    public Camera cam;
    private Rigidbody2D rb2d;
    public Vector3 force;

    [Header("Jumping")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpVelocity;
    public int jumpCount;
    public bool canJump;

    private float minX = 0f;
    private float maxX = 15f;
    private float minY = -2f;
    private float maxY = 4f;

    public AudioSource audioSource;


    [Header("GroundCheck")]
    public LayerMask groundLayer;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        canJump = false;
        jumpCount = 0;

        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        minY *= screenHeight / 1080;
        maxY *= screenHeight / 1080;
        minX *= screenWidth / 1920;
        maxX *= screenWidth / 1920;
    }

    private void Start()
    {
        cam = GameController.instance.cam;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartMovement();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndMovement();
            return;
        }

        ClampPlayer();
    }

    private void FixedUpdate()
    {
        Debug.Log(moving);
        if (moving)
        {
            Move();
        }

        if (jump && canJump)
        {
            Jump();
        }

        BetterJump();
        CheckGround();
    }

    private void ClampPlayer()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, minX, maxX);
        pos.y = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = pos;
    }

    private void StartMovement()
    {
        tap = true;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, 10);
        _movementStartPosition = cam.ScreenToWorldPoint(mousePosition);
        _movementCurrentPosition = _movementStartPosition;
        _movementLastPosition = _movementCurrentPosition;
        SetMoving();
    }

    private void EndMovement()
    {
        CheckTap();
        CancelInvoke("SetMoving");
        moving = false;
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }
    private void SetMoving()
    {
        moving = true;
    }

    private void Move()
    {
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, 10);
        //_movementCurrentPosition = cam.ScreenToWorldPoint(mousePosition);

        //Vector3 force = _movementStartPosition - _movementCurrentPosition;
        //if (Mathf.Abs(force.x) < Mathf.Abs(_lastdirection.x))
        //{
        //    rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        //    _movementStartPosition = _movementCurrentPosition;
        //    force = _movementStartPosition - _movementCurrentPosition;
        //}

        //rb2d.velocity = new Vector2(-force.x * playerSpeed * Time.fixedDeltaTime, rb2d.velocity.y);

        //_lastdirection = -force;

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, 10);
        _movementCurrentPosition = cam.ScreenToWorldPoint(mousePosition);

        force = _movementCurrentPosition - _movementLastPosition;

        rb2d.velocity = new Vector2(force.x * playerSpeed * Time.fixedDeltaTime, rb2d.velocity.y); ;

        _movementLastPosition = _movementCurrentPosition;
    }

    private void Tap()
    {
        if (jumpCount > 1)
        {
            canJump = false;
            return;
        }
        jumpCount += 1;
        jump = true;
    }

    private void CheckTap()
    {
        float distance = Vector2.Distance(_movementStartPosition, _movementCurrentPosition);
        if (distance < 0.1f)
        {
            Tap();
        }
    }

    private void Jump()
    {
        jump = false;
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
    }

    private void BetterJump()
    {
        if (rb2d.velocity.y > 0)
        {
            rb2d.gravityScale = lowJumpMultiplier;
        }else
        {
            rb2d.gravityScale = fallMultiplier;
        }
    }

    private void CheckGround()
    {
        //Debug.DrawRay(transform.position, Vector2.down * 1f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        if (hit.collider != null)
        {
            canJump = true;
            jumpCount = 0;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            GameController.instance.EndGame(transform.position);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            other.gameObject.SetActive(false);
            GameController.instance.AddBonus(other.transform.name, other.transform.position);
            audioSource.Play();
        }

        if (other.CompareTag("PowerUps"))
        {
            other.gameObject.SetActive(false);
            other.gameObject.GetComponentInParent<PowerUPsSpawner>().PowerUp(other.gameObject.name);
        }
    }
}
