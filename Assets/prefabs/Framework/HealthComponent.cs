using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnHealthChanged(int newValue, int oldValue, int maxValue, GameObject Causer);
public delegate void OnNoHealthLeft();

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public int HitPoints = 10;
    [SerializeField] public int MaxHitPoints = 10;

    public OnHealthChanged onHealthChanged;
    public OnNoHealthLeft noHealthLeft;

    public void TakeDamage(int damage, GameObject DamageCauser)
    {
        int oldValue = HitPoints;
        HitPoints -= damage;
        if(HitPoints <= 0)
        {
            HitPoints = 0;
            if(noHealthLeft!=null)
            {
                noHealthLeft.Invoke();
            }
        }
        if (onHealthChanged != null)
        {
            onHealthChanged.Invoke(HitPoints, oldValue, MaxHitPoints, DamageCauser);
        }
    }

    public void Heal(int recoverd, GameObject Healer)
    {
        int oldValue = HitPoints;
        HitPoints += recoverd;
        
        if (onHealthChanged != null)
        {
            onHealthChanged.Invoke(HitPoints, oldValue, MaxHitPoints, Healer);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Weapon attackingWeapon = other.GetComponentInParent<Weapon>();
        if(attackingWeapon!=null)
        {
            TakeDamage((int)(attackingWeapon.GetDamagePerBullet()), attackingWeapon.Owner);
        }
    }

    public void BroadCastCurrentHealth()
    {
        onHealthChanged.Invoke(HitPoints, HitPoints, MaxHitPoints, gameObject);
    }
}
