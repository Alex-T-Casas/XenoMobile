using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityBase : ScriptableObject
{
    [SerializeField] float CooldownTime = 5.0f;
    [SerializeField] Sprite Icon;
    [SerializeField] int AbilityLevel = 1;
    [SerializeField] float CooldownProgess = 1.0f;


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

    public float GetCooldownProgress()
    {
        return CooldownProgess;
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
        //CooldownProgess = -1 / CooldownTime * Time.deltaTime;
        CooldownProgess = 1.0f;
        float CooldownCounter = 0.0f;
        while(CooldownCounter < CooldownTime)
        {
            yield return new WaitForEndOfFrame();
            CooldownCounter += Time.deltaTime;
            CooldownProgess -= (1f / CooldownTime * Time.deltaTime);
        }
        //yield return new WaitForSeconds(CooldownTime);
        IsOnCooldown = false;
        CooldownProgess = 1.0f;
        yield return null;
    }

    internal void UseAbility()
    {
        throw new NotImplementedException();
    }
}
