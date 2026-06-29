using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordCollider : MonoBehaviour
{
    public ChainSwords CS;
    public bool isRight;
    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth EH = other.GetComponent<EnemyHealth>();

        if (EH != null)
        {
            EH.TakeDamage(transform.position, CS.slashSequence[CS.currentAttack - 1].damage);
        }
    }

    public void EnableCollider()
    {
        GetComponent<Collider>().enabled = true;
    }
    public void disableCollider()
    {
        GetComponent<Collider>().enabled = false;
    }
}
