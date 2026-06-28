using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;


    Vector3 inputDir;
    float horizontalInput;
    float verticalInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        if(playerMovement.instance.States != playerMovement.STATES.sliding)
        {
             horizontalInput = Input.GetAxis("Horizontal");
             verticalInput = Input.GetAxis("Vertical");            
        }

        inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

    }  
}
