using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    HealthComponent healthComp;
    // Start is called before the first frame update
    public virtual void Start()
    {
        healthComp = GetComponent<HealthComponent>();

        healthComp.onHealthChanged += HealthChanged;
        healthComp.noHealthLeft += NoHealthLeft;
    }

    public virtual void NoHealthLeft(GameObject killer = null)
    {
        GetComponent<Animator>().SetLayerWeight(2, 1);
        int DeathStateNameHash = Animator.StringToHash("DeathState");
        GetComponent<Animator>().Play(DeathStateNameHash);
    }

    public virtual void HealthChanged(float newValue, float oldValue, float maxValue, GameObject Caluse)
    {
        
    }

    public void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
