using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Shop/ShopSystem")]

public class ShopSystem : ScriptableObject
{
    [SerializeField] CreditManager creditManager;
    [SerializeField] Weapon[] weaponsOnSale;

    // Start is called before the first frame update
    void Start()
    {
        creditManager = FindObjectOfType<CreditManager>();
    }

    internal Weapon[] GetWaponsOnSale()
    {
        return weaponsOnSale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseWeapon(string WeaponName)
    {
        if (creditManager == null)
        {
            creditManager = FindObjectOfType<CreditManager>();
        }

        if(creditManager == null)
        {
            return;
        }


        foreach (Weapon weapon in weaponsOnSale)
        {
            if(weapon.GetWeaponInfo().name == WeaponName)
            {
                Player player = FindObjectOfType<Player>();
                if(player != null && CanPurchase(weapon.GetWeaponInfo().cost))
                {
                    player.AquireNewWeapon(weapon, true);
                    creditManager.ChangeCredits(-weapon.GetWeaponInfo().cost);
                }
            }
        }
    }

    public bool CanPurchase(float cost)
    {
        if(!HasCreditManager())
        {
            return false;
        }
        if(cost > creditManager.playerCredits)
        {
            return false;
        }
        return true;

    }

    bool HasCreditManager()
    {
        if(creditManager == null)
        {
            creditManager = FindObjectOfType<CreditManager>();
        }

        return creditManager != null;
    }
}
