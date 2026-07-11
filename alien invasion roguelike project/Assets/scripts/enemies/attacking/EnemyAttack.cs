using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("attacks")]
    public EnemyAttackSO[] attackList;

    [Header("floats")]
    public float cooldown;
    public float cooldownTimer;
    public float triggerDistance;

    [Header("bools")]
    public bool Attacking;
    public bool CanAttack;
    public bool inRange;

    [Header("references")]
    public Animator EnemyAnimator;
    EnemyReferences EF;

    public void Awake()
    {
        EF = GetComponent<EnemyReferences>();
    }
   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
}
