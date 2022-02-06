using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpeedBoost : AbilityBase
{
    [SerializeField] float TopSpeed = 2.5f;
    [SerializeField] float BoostTime = 3.0f;

    MovementComponent movementComponent;

    public override void Init(AbilityComponent ownerAbilityComponent)
    {
        base.Init(ownerAbilityComponent);
        movementComponent = OwnerComp.GetComponent<MovementComponent>();
    }

    public override void ActivateAbility()
    {
        if (CommitAbility())
        {
            OwnerComp.StartCoroutine(SpeedBoostCoroutine());
        }
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        float RegenCounter = 0.0f;
        while (RegenCounter < BoostTime)
        {
            yield return new WaitForEndOfFrame();
            RegenCounter += Time.deltaTime;
            movementComponent.SetMovementSpeed(TopSpeed / BoostTime * Time.deltaTime);
        }
    }
}
