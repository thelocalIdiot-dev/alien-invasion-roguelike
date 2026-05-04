using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    [Header("spring")]
    public float halflife = 0.075f;
    public float frequency = 18;

    public Vector3 springPosition;
    public Vector3 springVelocity;

    void Start()
    {
        springPosition = Vector3.zero;
        springVelocity = Vector3.zero;

    }

    private void Update()
    {
        Spring(ref springPosition, ref springVelocity, transform.position, halflife, frequency, Time.deltaTime);

        Vector3 localSpringPos = springPosition - transform.position;
        float springHeight = Vector3.Dot(localSpringPos, Vector3.up);

        transform.localEulerAngles = new Vector3(-springHeight * 2, 0, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, springPosition);
        Gizmos.DrawSphere(springPosition, 0.1f);
    }


    public void Spring(ref Vector3 current, ref Vector3 velocity, Vector3 target, float halfLife, float frequency, float timeStep)
    {
        float f = 1.0f + 2.0f * timeStep * halfLife * frequency;
        float oo = frequency * frequency;
        float hoo = timeStep * oo;
        float hhoo = timeStep * hoo;
        float detInv = 1.0f / (f + hhoo);
        Vector2 detX = f * current + timeStep * velocity + hhoo * target;
        Vector2 detV = velocity + hoo * (target - current);
        current = detX * detInv;
        velocity = detV * detInv;
    }
}
