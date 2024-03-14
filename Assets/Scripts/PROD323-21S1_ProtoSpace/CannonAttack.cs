using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAttack : MonoBehaviour
{
    [Range(20f, 200f)]
    public float attackDist = 200f;
    [Range(0.1f, 2f)]
    public float fireInterval = 1f;
    public GameObject laserBeam;
    public GameObject barrelEnd;
    private GameObject player;
    private float timer = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(timer > fireInterval) 
        {
            Attack();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

        void Attack()
    {
        Vector3 direction = player.transform.position - transform.position;

        float distance = direction.magnitude;
        if(distance < attackDist) {
            Fire(direction.normalized);
            return;
        }
    }

        void Fire(Vector3 v)
    {
        var transform = barrelEnd.transform;
        var laser = Instantiate(laserBeam, barrelEnd.transform.position, Quaternion.identity);
        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(90, 0, 0);
        laser.transform.localScale = laser.transform.localScale * 5f;
        laser.transform.rotation = transform.rotation * rot;
    }
}
