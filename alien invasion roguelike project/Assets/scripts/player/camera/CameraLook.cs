using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private float xRotation;
    public float sensitivity = 50f;
    private float sensMultiplier = 1f;
    public Transform orientation;
    public Transform player;

    private float desiredX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);

        if (!playerMovement.instance.sliding)
        {
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }
        transform.position = player.position;
    }    
}
