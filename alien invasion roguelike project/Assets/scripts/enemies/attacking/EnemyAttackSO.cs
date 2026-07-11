using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new enemy attack", menuName = "scriptable objects/enemy attack")]
public class EnemyAttackSO : ScriptableObject
{
    [Header("references")]
    public AnimatorOverrideController controller;
    [Header("values")]
    public Vector3 lungePower;
    public float attackDuration;
}
