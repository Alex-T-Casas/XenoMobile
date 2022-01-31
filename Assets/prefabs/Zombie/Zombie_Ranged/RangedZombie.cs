using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedZombie : Zombie
{
    //[SerializeField] Transform TossTransform;
    [SerializeField] GameObject projectile;
    public override void AttackPoint()
    {
        base.AttackPoint();
        /*GameObject target = GetComponent<AIController>().Target;
        Vector3 ThrowDir = TossTransform.forward;
        float Up = Vector3.Dot(ThrowDir, Vector3.up);
        float Forward = Vector3.Dot(ThrowDir, transform.forward);
        float distance = Vector3.Distance(target.transform.position, transform.position);
        float GravityAccel = Physics.gravity.magnitude;
        float Speed = Mathf.Sqrt(Mathf.Abs(GravityAccel * distance / (2 * Up * Forward)));
        Vector3 ThrowVel = Speed * ThrowDir;
        GameObject newProjectile = Instantiate(projectile, TossTransform.position, TossTransform.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(ThrowVel, ForceMode.VelocityChange);*/
        Instantiate(projectile);

    }
}
