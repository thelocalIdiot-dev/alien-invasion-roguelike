using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyTurn : MonoBehaviour
{
    public float lookSpeed;
    public Vector3 offset;
    Rigidbody rb;
    [Header("lock")]
    public bool lockUp;

    public EnemyHealth health;
    public bool predictMovement = true, grounded;
    Vector3 predictedPos;
    Vector3 lookDirection;

    EnemyReferences EF;
    void Awake()
    {
        EF = GetComponent<EnemyReferences>();

        rb = GetComponent<Rigidbody>();
        health = GetComponent<EnemyHealth>();
        if (health == null)
        {
            health = GetComponentInParent<EnemyHealth>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        EnemyNavigation EN = GetComponent<EnemyNavigation>();

        if (EN != null)
        {
            grounded = GetComponent<EnemyNavigation>().Grounded();
        }
        else
        {
            grounded = true;
        }
       

        if (!health.stuned && grounded)
        {
            Vector3 playerPosition = playerMovement.instance.transform.position + offset;
            

            Vector3 flatDir = new Vector3(lookDirection.x, 0, lookDirection.z);

            lookDirection = playerPosition - transform.position;


            if (rb != null)
            {
                if (lockUp)
                {
                    rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(flatDir), lookSpeed));
                }
                else
                {
                    rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed));
                }
            }
            else
            {
                if (lockUp)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(flatDir), lookSpeed);
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed);
                }
            }
            
        }

    }

    private Vector3 predictedPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = targetPosition - shooterPosition;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            return targetPosition;
        }
        //also Sine Formula
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }
}
