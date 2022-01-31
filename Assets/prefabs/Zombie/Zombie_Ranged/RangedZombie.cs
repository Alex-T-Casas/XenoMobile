using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedZombie : Zombie
{
    [SerializeField] Vector3 leftHand;
    [SerializeField] GameObject projectile;
    public override void AttackPoint()
    {
        base.AttackPoint();

        Instantiate(projectile);

    }
}
