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

   
        void Start()
    {
        player = GameObject.Find("Player");
        SetPlayersLastPos();
        startPos = GameObject.Find("Zombie_Ranged").transform;
    }

    private void Update()
    {
        Velocity += Time.deltaTime * projSpeed;
        float height = flightCurve.Evaluate(Velocity);
        Vector3 originWithHeight = new Vector3(startPos.position.x, startPos.position.y + height, startPos.position.z);
        Vector3 targetWithHeight = new Vector3(targetPos.x, targetPos.y + height, targetPos.z);
        transform.position = Vector3.Lerp(originWithHeight, targetWithHeight, Velocity);

    }

    private void SetPlayersLastPos ()
    {
        Vector3 playersPos = player.transform.position;

        targetPos = playersPos;
    }
}
