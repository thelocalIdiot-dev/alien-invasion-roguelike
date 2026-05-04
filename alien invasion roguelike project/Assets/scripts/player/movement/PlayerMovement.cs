using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    [Header("----------MOVEMENT----------")]
    [Header("---Ground movement---")]
    public float runSpeed = 10;
    public float groundDrag = 5;
    public float playerheight = 1.5f;
    public LayerMask WhatIsGround;
    public Vector3 moveDir;
    float moveSpeed;
    float desiredMoveSpeed;
    float LastDesiredMoveSpeed;
    [Header("---Slope movement---")]
    public float slideSpeed = 60;
    public float maxSlopeAngle = 50;
    RaycastHit Slopehit;
    public bool exitingSlope;
    [Header("---Wall running---")]
    public float wallrunSpeed = 500;
    [Header("---Air movement---")]
    [Header("jumping")]
    public float jumpforce = 12;
    public float jumpCooldown = .5f;
    public float airMultiplier = 0.4f;
    public bool canDoubleJump;
    public bool readyToJump;
    [Header("input")]
    public float horizontalInput;
    public float verticalInput;
    bool jumping;
    [Header("----------REFRENCES----------")]
    public Transform orientation;
    public Rigidbody rb;
    [Header("----------STATES----------")]
    public bool sliding;
    public bool dashing;
    public bool wallrunning;
    public enum STATES { idle, walking, air, dashing, sliding, wallrunning };
    public STATES States;
    [Header("----------EFFECTS----------")]
    [Header("---VFX---")]
    public GameObject jumpSmoke;

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetFallSpeed()
    {
        return this.rb.velocity.y;
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }


    public static playerMovement instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        readyToJump = true;
    }

    private void Update()
    {
        GetInput();
        stateHandler();

        if (States == STATES.idle || States == STATES.walking)
        {
            rb.drag = groundDrag;
        }
        else if (States != STATES.idle && States != STATES.walking)
        {
            rb.drag = 0f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            restartGame();
        }
    }

    void stateHandler()
    {

        if (sliding)
        {
            States = STATES.sliding;
            desiredMoveSpeed = slideSpeed;
            CameraEffects.instance.changeFOV(CameraEffects.instance.slidingFOV, 0.25f);
            CameraEffects.instance.tilt(CameraEffects.instance.slidingTilt * horizontalInput, 0.25f);
        }
        else if (dashing)
        {
            States = STATES.dashing;
        }
        else if (wallrunning)
        {
            States = STATES.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }
        else if (grounded() && (horizontalInput != 0 || verticalInput != 0))
        {
            CameraEffects.instance.tilt(CameraEffects.instance.strafTilt * -horizontalInput, 0.25f);
            States = STATES.walking;
            desiredMoveSpeed = runSpeed;
        }
        else if (grounded())
        {
            States = STATES.idle;
            desiredMoveSpeed = runSpeed;
            CameraEffects.instance.changeFOV(CameraEffects.instance.baseFOV, 0.25f);
            CameraEffects.instance.tilt(0, 0.25f);

        }
        else if (!grounded())
        {
            States = STATES.air;
            CameraEffects.instance.tilt(CameraEffects.instance.strafTilt * -horizontalInput, 0.25f);
        }

        if (moveSpeed != 0)
        {
            StartCoroutine(smoothlyChangeSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
    }

    private IEnumerator smoothlyChangeSpeed()
    {
        float time = 0f;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }


    private void FixedUpdate()
    {
        MovePlayer();
        speedControl();
    }
    void GetInput()
    {
        if (States != STATES.dashing && States != STATES.sliding && States != STATES.wallrunning)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        //else
        //{
        //    horizontalInput = 0;
        //    verticalInput = 0;
        //}

        if (Input.GetKey(KeyCode.Space) && grounded() && readyToJump)
        {
            jump();

            readyToJump = false;

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump && States != STATES.wallrunning)
        {
            jump();

            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);

            canDoubleJump = false;
        }
        if (grounded())
        {
            canDoubleJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, playerheight * 0.5f + 0.2f, WhatIsGround);

        if (grounded())
        {
            GameObject JS = Instantiate(jumpSmoke, hit.point, Quaternion.Euler(-90, 0, 0));
            Destroy(JS, 2f);
        }
    }

    void MovePlayer()
    {
        moveDir = orientation.right * horizontalInput + orientation.forward * verticalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y < 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (grounded())
            rb.AddForce(moveDir.normalized * moveSpeed * 10, ForceMode.Force);
        else
            rb.AddForce(moveDir.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope() && !dashing;
    }

    public bool grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerheight * 0.5f + 0.2f, WhatIsGround);
    }
    void jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
    }
    void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }
    void speedControl()
    {
        if (States != STATES.dashing)
        {
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                    rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }
    }

    public bool OnSlope()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out Slopehit, playerheight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, Slopehit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }


    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, Slopehit.normal).normalized;
    }
}
