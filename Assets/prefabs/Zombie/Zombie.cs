using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Zombie : Character
{
    NavMeshAgent navAgent;
    Animator animator;
    Rigidbody ZombieRigidbody;
    float speed;
    Vector3 previousLocation;

    [SerializeField] float StaminaReward = 0.5f;
    [SerializeField] int CreditReward = 5;

    [SerializeField] GameObject creditManager;

    // Start is called before the first frame update
    public override void Start() 
    {
        navAgent = GetComponent<NavMeshAgent>();
        base.Start();
        animator = GetComponent<Animator>();
        ZombieRigidbody = GetComponent<Rigidbody>();
        previousLocation = transform.position;
    }

    internal void Attack()
    {
        animator.SetLayerWeight(1, 1);
    }
    public virtual void AttackPoint()
    {
        
    }
    public void AttackFinished()
    {
        animator.SetLayerWeight(1, 0);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        float MoveDelta = Vector3.Distance(transform.position, previousLocation);
        speed = MoveDelta / Time.deltaTime;
        previousLocation = transform.position; 
        animator.SetFloat("Speed", speed);
    }

    public override void NoHealthLeft(GameObject killer)
    {
        base.NoHealthLeft();
        AIController AIC = GetComponent<AIController>();
        if(AIC != null)
        {
            AIC.StopAIBehavior();
        }
        if(killer != null)
        {
            AbilityComponent abilityComponent = killer.GetComponent<AbilityComponent>();
            if(abilityComponent)
            {
                abilityComponent.ChangeStamina(StaminaReward);
                creditManager.GetComponent<CreditManager>().ChangeCredits(CreditReward);
            }
        }

    }
}
