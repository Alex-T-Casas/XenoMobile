using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthRegen : AbilityBase
{
    [SerializeField] float RegenAmount = 2.5f;
    [SerializeField] float RegenTime = 3.0f;

    HealthComponent healthComponent;

    public override void Init(AbilityComponent ownerAbilityComponent)
    {
        base.Init(ownerAbilityComponent);
        healthComponent = OwnerComp.GetComponent<HealthComponent>();
    }
    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            OwnerComp.StartCoroutine(HealthRegenCoroutine());
        }
    }

    private IEnumerator HealthRegenCoroutine()
    {
        float RegenCounter = 0.0f;
        while(RegenCounter < RegenTime)
        {
            yield return new WaitForEndOfFrame();
            RegenCounter += Time.deltaTime;
            healthComponent.ChangeHealth(RegenAmount / RegenTime * Time.deltaTime, healthComponent.gameObject);
        }
    }
}
