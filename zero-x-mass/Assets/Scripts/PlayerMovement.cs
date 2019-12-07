using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public bool moving;
    public bool jump;
    public bool tap;

    public Vector3 _movementStartPosition;
    public Vector3 _movementCurrentPosition;
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

    [Header("GroundCheck")]
    public LayerMask groundLayer;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        canJump = false;
        jumpCount = 0;
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
        pos.x = Mathf.Clamp(transform.position.x, -5.0f, 6.0f);
        pos.y = Mathf.Clamp(transform.position.y, -4.0f, 4.0f);
        transform.position = pos;
    }

    private void StartMovement()
    {
        tap = true;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, 10);
        _movementStartPosition = cam.ScreenToWorldPoint(mousePosition);
        _movementCurrentPosition = _movementStartPosition;
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
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, 0, 10);
        _movementCurrentPosition = cam.ScreenToWorldPoint(mousePosition);

        Vector3 force = _movementStartPosition - _movementCurrentPosition;
        if (Mathf.Abs(force.x) < Mathf.Abs(_lastdirection.x))
        {
            rb2d.velocity = Vector3.zero;
            _movementStartPosition = _movementCurrentPosition;
            force = _movementStartPosition - _movementCurrentPosition;
        }

        rb2d.velocity = new Vector2(-force.x * GameController.instance.playerSpeed * Time.fixedDeltaTime, rb2d.velocity.y);

        _lastdirection = -force;
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
        Debug.Log(_movementCurrentPosition + "current");
        Debug.Log(_movementStartPosition + "start");
        if (_movementCurrentPosition == _movementStartPosition)
        {
            Tap();
        }
    }

    private void _CheckTap()
    {
        if (tap)
        {
            if (_movementCurrentPosition != _movementStartPosition)
            {
                tap = false;
            }
        }
    }

    private void Jump()
    {
        jump = false;
        //GameController.instance.scrollSpeed *= 1.25f;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.85f, groundLayer);
        if (hit.collider != null)
        {
            canJump = true;
            jumpCount = 0;
        }
    }
}
