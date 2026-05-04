using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashing : MonoBehaviour
{
    public float dashForce = 10f;
    public float dashCooldown = 0.2f;
    public Transform orientation;

    private Rigidbody rb;
    private bool canDash = true;
    private Vector3 dashDirection;
    private playerMovement pm;
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        pm = GetComponent<playerMovement>();
    }

    void Update()
    {
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashDirection = pm.moveDir;

            Dash(dashDirection);

            
        }

       if (pm.dashing)
       {
            CameraEffects.instance.changeFOV(CameraEffects.instance.dashingFOV, 0.25f);
       }
       else
       {
            CameraEffects.instance.changeFOV(CameraEffects.instance.baseFOV, 0.25f);
       }
    }

    void Dash(Vector3 direction)
    {
        pm.rb.velocity = Vector3.zero;
        rb.AddForce(direction * dashForce, ForceMode.VelocityChange);
        canDash = false;
        pm.dashing = true;
        pm.exitingSlope = true;
        Invoke(nameof(ResetDash), dashCooldown);
        

    }

    void ResetDash()
    {
        canDash = true;
        pm.dashing = false;
        pm.exitingSlope = false;
    }
}
