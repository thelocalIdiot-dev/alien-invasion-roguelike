using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new attack", menuName = "scriptable objects/attack")]
public class attackSO : ScriptableObject
{
    [Header("references")]
    public AnimatorOverrideController controller;
    [Header("values")]
    public float duration;
    public float damage;
    public bool right;

}
