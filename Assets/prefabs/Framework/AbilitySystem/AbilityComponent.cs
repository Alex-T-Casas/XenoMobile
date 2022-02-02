using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnNewAblilityInitialized(AbilityBase newAbility);

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] float Steminalevel;
    [SerializeField] float MaxStaminaLevel;

    [SerializeField] AbilityBase[] abilities;

    public event OnNewAblilityInitialized onNewAbilityInitialzed;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);
            onNewAbilityInitialzed?.Invoke(abilities[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal int GetSteminaLevel()
    {
        return (int)Steminalevel;
    }
}
