using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerZeroGMovement : MonoBehaviour
{
    public float playerSpeed = 2000f;
    public bool moving;
    public bool jump;


    public Vector3 _movementStartPosition;
    public Vector3 _movementCurrentPosition;
    public Vector3 _movementLastPosition;
    public Vector3 _lastdirection;
    public Vector2 _goPosition;

    public Camera cam;
    private Rigidbody2D rb2d;
    public Vector3 force;

    private float minX = 0f;
    private float maxX = 10f;
    private float minY = -4f;
    private float maxY = 4f;

    public TMP_Text debugger;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        minY *= screenHeight / 1080;
        maxY *= screenHeight / 1080;
        minX *= screenWidth / 1920;
        maxX *= screenWidth / 1920;
        debugger.text = maxY + "";
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
        //RotateToTarget();
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
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        _movementStartPosition = cam.ScreenToWorldPoint(mousePosition);
        _movementCurrentPosition = _movementStartPosition;
        _movementLastPosition = _movementCurrentPosition;
        SetMoving();
    }

    private void EndMovement()
    {
        CancelInvoke("SetMoving");
        moving = false;
        rb2d.velocity = Vector3.zero;

        Vector3 pushForce = new Vector3(Random.Range(0f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        rb2d.velocity = pushForce;
    }

    private void SetMoving()
    {
        moving = true;
    }

    private void Move()
    {
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        //_movementCurrentPosition = cam.ScreenToWorldPoint(mousePosition);


        //force = _movementStartPosition - _movementCurrentPosition;
        //if (Mathf.Abs(force.x) < Mathf.Abs(_lastdirection.x)
        //|| Mathf.Abs(force.y) < Mathf.Abs(_lastdirection.y))
        //{
        //    rb2d.velocity = Vector3.zero;
        //    _movementStartPosition = _movementCurrentPosition;
        //    force = _movementStartPosition - _movementCurrentPosition;
        //}

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        _movementCurrentPosition = cam.ScreenToWorldPoint(mousePosition);

        force = _movementCurrentPosition - _movementLastPosition;

        rb2d.velocity = force * playerSpeed * Time.fixedDeltaTime;

        _lastdirection = -force;

        _movementLastPosition = _movementCurrentPosition;

    }

    public float rotationSteepSpeed;
    private void RotateToTarget()
    {
        _goPosition = (Vector2)transform.position + (Vector2)force;

        var dir = _goPosition - rb2d.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;

        if (angle > 170 && angle < 190)
        {
            return;
        }

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSteepSpeed * Time.fixedDeltaTime);

        //rb2d.rotation =  Mathf.LerpAngle(rb2d.rotation, angle, rotationSteepSpeed * Time.fixedDeltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
