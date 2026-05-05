using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    private Rigidbody rb;
    private playerMovement pm;
    public GameObject legs;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;
    Vector3 inputDirection;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<playerMovement>();

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        if (!pm.sliding && !pm.dashing)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if (Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();

        if (Input.GetKeyDown(slideKey) && !pm.grounded())
            rb.velocity = new Vector3(rb.velocity.x, -12, rb.velocity.z);
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;

        transform.DOScale(new Vector3(transform.localScale.x, slideYScale, transform.localScale.z), 0.25f);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;

        transform.DOScale(new Vector3(transform.localScale.x, startYScale, transform.localScale.z), 0.25f);
    }
}
