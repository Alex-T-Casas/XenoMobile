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

    public virtual void NoHealthLeft()
    {
        GetComponent<Animator>().SetLayerWeight(2, 1);
        int DeathStateNameHash = Animator.StringToHash("DeathState");
        GetComponent<Animator>().Play(DeathStateNameHash);

        StopAILogic();
        StopPlayerInput();

        // when anim is finnished, destroy gameobj
    }

    public virtual void HealthChanged(int newValue, int oldValue, int maxValue, GameObject Caluse)
    {
        
    }
    private void StopAILogic()
    {
       AIController AIC = GetComponent<AIController>();
        if(AIC != null)
        {
            AIC.StopAIBehavior();
        }
    }

    private void StopPlayerInput()
    {
        Player player = GetComponent<Player>();
        if(player != null)
        {
            player.StopPlayerBehavior();
        }
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
