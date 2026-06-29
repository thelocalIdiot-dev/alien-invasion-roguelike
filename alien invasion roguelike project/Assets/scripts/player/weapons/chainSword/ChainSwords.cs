using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSwords : MonoBehaviour
{
    public Animator animator;

    [Header("delays")]
    public float equippingTime;
    [Header("timer and times")]
    public float cooldownTime;
    public float comboTime;
    public float Timer;

    public attackSO[] slashSequence;

    public int currentAttack = 1;

    public bool attacking;
    public bool canAttack;

    public swordCollider[] swords;
    public eventReceiver ER;

    public static ChainSwords instance;

    public void Awake()
    {
        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].disableCollider();
        }

        instance = this;
    }
   //
   // void equipe()
   // {
   //     animator.SetTrigger("equipe");
   // }

    private void Update()
    {
        Timer += Time.deltaTime;
        
        if(Input.GetMouseButtonDown(0))
        {
            attack();
        }
        manageColliders();
    }

    void attack()
    {        
        if (canAttack && !attacking && playerMovement.instance.States != playerMovement.STATES.air && playerMovement.instance.States != playerMovement.STATES.sliding)
        {
            canAttack = false;
            attacking = true;

            if (Timer < comboTime)
            {
                currentAttack++;
                if (currentAttack > slashSequence.Length)
                {
                    currentAttack = 1;

                }
                Timer = 0;
            }
            else
            {
                currentAttack = 1;
                Timer = 0;
            }

            animator.runtimeAnimatorController = slashSequence[currentAttack - 1].controller;
            if(currentAttack == 1)
            {
                animator.Play("Attack", 0, 0);
            }
            else
            {
                animator.CrossFadeInFixedTime("Attack", 0.1f, 0);
            }
            
        }
    }

    void manageColliders()
    {
        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].GetComponent<Collider>().enabled = ER.isOn;
        }
    }

    public void ResetAttack()
    {    
        canAttack = true;
        attacking = false;
    }
}
