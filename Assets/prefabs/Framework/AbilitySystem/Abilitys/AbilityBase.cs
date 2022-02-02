using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    [SerializeField] float CooldownTime = 5.0f;
    [SerializeField] Sprite Icon;
    [SerializeField] int AbilityLevel = 1;

    public AbilityComponent OwnerComp
    {
        get;
        set;
    }

    public bool IsOnCooldown
    {
        private set;
        get;
    }
    public Sprite GetIcon()
    {
        return Icon;
    }
    public int GetLevel()
    {
        return AbilityLevel;
    }

    public virtual void Init(AbilityComponent onwerAbilityComp)
    {
        OwnerComp = onwerAbilityComp;
    }

    bool CanCast()
    {
        return !IsOnCooldown && OwnerComp.GetSteminaLevel() >= AbilityLevel;
    }

    public abstract void ActivateAbility();
    protected bool CommitAbility()
    {
        if(CanCast())
        {
            StartCooldown();
            return true;
        }
        return false;
    }

    private void StartCooldown()
    {
        OwnerComp.StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        IsOnCooldown = false;
        yield return null;
    }

    internal void UseAbility()
    {
        throw new NotImplementedException();
    }
}
