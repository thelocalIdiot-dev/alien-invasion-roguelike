using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReferences : MonoBehaviour
{
    [Header("all scripts")]
    public EnemyAttack _EnemyAttack;
    public EnemyNavigation _EnemyNavigation;
    public EnemyTurn _EnemyTurn;
    public EnemyHealth _EnemyHealth;
    
    [Header("bools")]
    public bool InRange;

    public void Awake()
    {
        _EnemyAttack = GetComponent<EnemyAttack>();
        _EnemyNavigation = GetComponent<EnemyNavigation>();
        _EnemyHealth = GetComponent<EnemyHealth>();
        _EnemyTurn = GetComponent<EnemyTurn>();
    }

    private void Update()
    {
        InRange = Vector3.Distance(_EnemyNavigation.target.position, transform.position) < _EnemyAttack.triggerDistance;
    }
}
