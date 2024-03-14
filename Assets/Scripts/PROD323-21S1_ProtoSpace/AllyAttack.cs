using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAttack : MonoBehaviour
{
    [Range(20f, 200f)]
    public float attackDist = 100f;
    [Range(0.1f, 2f)]
    public float fireInterval = 0.5f;
    public GameObject laserBeam;
    public GameObject beamer_R;
    public GameObject beamer_L;

    private GameObject player;
    private float timer = 0;
   private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();
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
        Vector3 direction = transform.forward;
        float distance = attackDist;

        var enemyList = GameObject.FindGameObjectsWithTag("enemy");

        for(int i = 0; i < enemyList.Length; i++)
        {
            Vector3 dir = enemyList[i].transform.position - transform.position;
            if(dir.magnitude < distance)
            {
                distance = dir.magnitude;
                direction = dir;
            }
        }
        
        if(distance < attackDist) {
            LookWhereFiring(direction.normalized);
            Fire(direction.normalized);
            return;
        }
    }

    void Fire(Vector3 dir)
    {
        var laserR = Instantiate(laserBeam, beamer_R.transform.position, Quaternion.LookRotation(dir, beamer_R.transform.up) * Quaternion.Euler(90, 0, 0));
        var laserL = Instantiate(laserBeam, beamer_L.transform.position, Quaternion.LookRotation(dir, beamer_R.transform.up) * Quaternion.Euler(90, 0, 0));
    }

    void LookWhereFiring(Vector3 direction)
    {
        transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(direction, Vector3.up), transform.rotation, Time.deltaTime * 10);
    }
}
