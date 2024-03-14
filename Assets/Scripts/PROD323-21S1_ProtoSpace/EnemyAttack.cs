using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("ally");
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

        var allyList = GameObject.FindGameObjectsWithTag("ally");

        for(int i = 0; i < allyList.Length; i++)
        {
            Vector3 dir = allyList[i].transform.position - transform.position;
            if(dir.magnitude < distance)
            {
                distance = dir.magnitude;
                direction = dir;
            }
        }
        
        if(distance < attackDist) {
            Fire(direction.normalized);
            return;
        }
    }

    void Fire(Vector3 dir)
    {
        var laserR = Instantiate(laserBeam, beamer_R.transform.position, Quaternion.LookRotation(dir, beamer_R.transform.up) * Quaternion.Euler(90, 0, 0));
        var laserL = Instantiate(laserBeam, beamer_L.transform.position, Quaternion.LookRotation(dir, beamer_R.transform.up) * Quaternion.Euler(90, 0, 0));
        
    }
}
