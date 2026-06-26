using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedLines : MonoBehaviour
{
    public float decrease;

    public float Time;
    void Update()
    {            
        Vector3 desiredPos = playerMovement.instance.transform.position + playerMovement.instance.rb.velocity * decrease;

        if (playerMovement.instance.transform.GetComponent<sliding>().effect)
        {
            GetComponent<ParticleSystem>().Play();
        }
        else
        {
            GetComponent<ParticleSystem>().Stop();
        }


        transform.position = desiredPos;

        transform.LookAt(playerMovement.instance.transform.position);
    }
}
