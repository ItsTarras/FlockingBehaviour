using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyShip;
    public GameObject spawnPoint;
    public float spawnInterval = 5f;
    public int maxShips = 10;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > spawnInterval) 
        {
            CheckTotalEnemy();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
   
    }

    void CheckTotalEnemy()
    {
        var shipList = GameObject.FindGameObjectsWithTag("enemy");
        //Debug.Log(shipList.Length);
        if(shipList.Length < maxShips)
            Spawn();
    }

    void Spawn()
    {
        if(spawnPoint != null)
            Instantiate(enemyShip, spawnPoint.transform.position, Quaternion.identity);
    }
}
