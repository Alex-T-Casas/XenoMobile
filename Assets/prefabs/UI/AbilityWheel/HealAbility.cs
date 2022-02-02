using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : MonoBehaviour
{
    [SerializeField] Player player;
    private float Health;
    private float MaxHealth;
    private bool OnCooldown;
    [SerializeField] float AbilityCooldown = 10.0f;
    //[SerializeField] int HealRate = 2; //per second
    //[SerializeField] float HealAmt = 5.0f;
    private bool isHealing;

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = player.GetComponent<HealthComponent>().MaxHitPoints;
    }
    private void Update()
    {
        if (!isHealing)
        {
            StopCoroutine(AddHealth());
        }
    }

    public void UseAbility()
    {
        if (!OnCooldown)
        {
            Debug.Log("Healing");
            Heal();
            Health = player.GetComponent<HealthComponent>().HitPoints;
            
            //Cooldown();
        }
        else
        {
            Debug.Log("Ability on cooldown");
        }

    }

    IEnumerator CooldownCoroutine()
    {
        OnCooldown = true;
        Debug.Log("ON COOLDOWN");
        yield return new WaitForSeconds(AbilityCooldown);
        OnCooldown = false;
        Debug.Log("OFF COOLDOWN");
    }

    public void Cooldown()
    {
        StartCoroutine(CooldownCoroutine());
    }

    public void Heal()
    {
        isHealing = true;
        StartCoroutine(AddHealth());

    }

    IEnumerator AddHealth()
    {
        while (!OnCooldown)
        {
            Health = player.GetComponent<HealthComponent>().HitPoints;
            if (Health < MaxHealth && isHealing)
            {
                player.GetComponent<HealthComponent>().ChangeHealth(1, gameObject); // increase health and wait the specified time
                yield return new WaitForSeconds(0.10f);
            }
            else
            {
                isHealing = false;
                Cooldown();
                yield return null;
            }
        }
    }
}
