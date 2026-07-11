using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyNavigation : MonoBehaviour
{
    [HideInInspector] public Transform target;
    public float speed, groundDetectionDistance, groundDrag;
    public Transform detectionOrigin;
    public Animator animator;
    public LayerMask ground;

    public bool inRange;

    Rigidbody rb;

    EnemyReferences EF;
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        EF = GetComponent<EnemyReferences>();
    }

    private void FixedUpdate()
    {        
        if (Grounded())
        {
            if (!EF.InRange)
            {
                Vector3 dir = (target.position - transform.position).normalized;
                Vector3 flatDir = new Vector3(dir.x, 0, dir.z);
                rb.AddForce(flatDir * speed * 10 * Time.fixedDeltaTime, ForceMode.Force);
                rb.constraints = RigidbodyConstraints.FreezeRotationX;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ;
                rb.drag = groundDrag;
                rb.useGravity = false;
            }                    
        }
        else
        {
            rb.drag = 0;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            rb.useGravity = true;
        }


        animator.SetFloat("Speed", rb.velocity.magnitude / 13);
    }
    public bool Grounded()
    {
        if (detectionOrigin == null)
        {
            return Physics.Raycast(transform.position, Vector3.down, groundDetectionDistance, ground);
        }
        else
        {
            return Physics.Raycast(detectionOrigin.position, Vector3.down, groundDetectionDistance, ground);
        }

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (detectionOrigin == null)
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDetectionDistance);
        }
        else
        {
            Gizmos.DrawLine(detectionOrigin.position, detectionOrigin.position + Vector3.down * groundDetectionDistance);
        }

    }
    
}
