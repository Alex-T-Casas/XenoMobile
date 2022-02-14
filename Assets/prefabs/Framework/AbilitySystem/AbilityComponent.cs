using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnNewAblilityInitialized(AbilityBase newAbility);
public delegate void OnStaminaUpdated(float newValue);

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] float Staminalevel;
    [SerializeField] float MaxStaminaLevel;
    [SerializeField] float StaminaDropRate = 0.5f;
    [SerializeField] float StaminaDrainDelay = 2.0f;

    [SerializeField] AbilityBase[] abilities;

    public event OnNewAblilityInitialized onNewAbilityInitialzed;
    public event OnStaminaUpdated onStaminaUpdated;

    Coroutine staminaDrainingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);
            onNewAbilityInitialzed?.Invoke(abilities[i]);
        }
        staminaDrainingCoroutine = StartCoroutine(StaminaDrainingCoroutine());
    }

    public void ChangeStamina(float ChangeValue)
    {
        if(ChangeValue > 0)
        {
            if (staminaDrainingCoroutine != null)
            {
                StopCoroutine(staminaDrainingCoroutine);
                Staminalevel = Staminalevel + ChangeValue;
                staminaDrainingCoroutine = StartCoroutine(StaminaDrainingCoroutine());
            }
        }
        Staminalevel = Mathf.Clamp(Staminalevel, 0, MaxStaminaLevel);
        onStaminaUpdated?.Invoke(Staminalevel);
    }

    IEnumerator StaminaDrainingCoroutine()
    {
        yield return new WaitForSeconds(StaminaDrainDelay);
        while(Staminalevel > 0)
        {

            Staminalevel -= StaminaDropRate * Time.deltaTime;
            onStaminaUpdated?.Invoke(Staminalevel);
            yield return new WaitForEndOfFrame();
        }
        Staminalevel = Mathf.Clamp(Staminalevel, 0, MaxStaminaLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if(Staminalevel > 0)
        {
            Staminalevel -= StaminaDropRate * Time.deltaTime;
            onStaminaUpdated?.Invoke(Staminalevel);
        }
    }

    internal float GetSteminaLevel()
    {
        return Staminalevel;
    }
}
