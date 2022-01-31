using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Script : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float projSpeed;
    [SerializeField] private AnimationCurve flightCurve;
    [SerializeField] Transform startPos;
    private Vector3 targetPos;
    private float Velocity = 0f;
    [SerializeField] float blastRadius = 0.5f;
    private float distFromPlayer;

   
        void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            SetPlayersLastPos();
        }
        startPos = GameObject.Find("Zombie_Ranged").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            Velocity += Time.deltaTime * projSpeed;
            float height = flightCurve.Evaluate(Velocity);
            Vector3 originWithHeight = new Vector3(startPos.position.x, startPos.position.y + height, startPos.position.z);
            Vector3 targetWithHeight = new Vector3(targetPos.x, targetPos.y + height, targetPos.z);
            transform.position = Vector3.Lerp(originWithHeight, targetWithHeight, Velocity);

            if (Velocity >= 1 && transform.position.y <= 0.2) //transform.position.y <= 0.12
            {
                Explode();
            }
            else if (Velocity >= 8)
            {
                Explode();
            }
        }
        else
        {
            Explode();
        }

    }

    private void Explode()
    {
        if (player != null)
        {
            GetDistFromPlayer();
            if (distFromPlayer <= blastRadius)
            {
                player.GetComponent<HealthComponent>().TakeDamage(2, gameObject);
            }
        }
        Debug.Log("Boom!");
        Destroy(gameObject);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, blastRadius);
    }*/

    private void GetDistFromPlayer()
    {
        distFromPlayer = (player.transform.position - gameObject.transform.position).magnitude;
    }

    private void SetPlayersLastPos ()
    {
        Vector3 playersPos = player.transform.position;

        targetPos = playersPos;
    }


}
