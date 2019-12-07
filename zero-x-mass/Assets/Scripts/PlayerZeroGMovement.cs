using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZeroGMovement : MonoBehaviour
{
    public bool moving;
    public bool jump;
    //public bool tap;

    public Vector3 _movementStartPosition;
    public Vector3 _movementCurrentPosition;
    public Vector3 _lastdirection;
    public Vector2 _goPosition;

    public Camera cam;
    private Rigidbody2D rb2d;
    public Vector3 force;

    private float lastAngle = 0;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
        //else if (Input.GetMouseButton(0))
        //{
        //    _CheckTap();
        //    _CheckJump();
        //}
        ClampPlayer();
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Move();
        }
    }

    private void ClampPlayer()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -2.0f, 6.0f);
        pos.y = Mathf.Clamp(transform.position.y, -2.0f, 4.0f);
        transform.position = pos;
    }

    private void StartMovement()
    {
        //tap = true;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        _movementStartPosition = cam.ScreenToWorldPoint(mousePosition);
        _movementCurrentPosition = _movementStartPosition;
        //Invoke("SetMoving", 0.1f);
        SetMoving();
    }

    private void EndMovement()
    {
        CancelInvoke("SetMoving");
        moving = false;
        rb2d.velocity = Vector3.zero;

        Vector3 test = new Vector3(Random.Range(0f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        rb2d.velocity = test;
        //if (tap)
        //{
        //    //just in case Dash();
        //}
    }

    private void SetMoving()
    {
        moving = true;
    }

    private void Move()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        _movementCurrentPosition = cam.ScreenToWorldPoint(mousePosition);



        force = _movementStartPosition - _movementCurrentPosition;
        if (Mathf.Abs(force.x) < Mathf.Abs(_lastdirection.x)
        || Mathf.Abs(force.y) < Mathf.Abs(_lastdirection.y))
        {
            Debug.Log("now");
            rb2d.velocity = Vector3.zero;
            _movementStartPosition = _movementCurrentPosition;
            force = _movementStartPosition - _movementCurrentPosition;
        }

        rb2d.velocity = -force * GameController.instance.playerSpeed * Time.fixedDeltaTime;

        _goPosition = (Vector2) transform.position + (Vector2) force;
        _lastdirection = -force;

        RotateToTarget();
    }

    public float rotationSteepSpeed;
    private void RotateToTarget()
    {
        Debug.Log(lastAngle + "last angle");


        var dir = _goPosition - rb2d.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;

        Debug.Log("angle " + angle);
        if (angle > 150 && angle < 210)
        {
            return;
        }
        rb2d.rotation =  Mathf.LerpAngle(rb2d.rotation, angle, rotationSteepSpeed * Time.fixedDeltaTime);
        lastAngle = angle;
    }
}
