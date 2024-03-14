using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBehaviour : MonoBehaviour
{
    [Range(10.0f, 20.0f)]
    [SerializeField] float perceptibleDist; //Distance that the rat detects the target character
    [Range(0.0f, 10.0f)]
    [SerializeField] float hostileDist; // Distance that the rat becomes aggressive and run toward the target
    [SerializeField] bool isNormal = true;
    Animator anim;
    Flock flock;
    FlockingManager manager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        flock = GetComponent<Flock>();
        //manager = flock.Manager;
    }

    // Overwrite changes in Flock.cs's Update using LateUpdate
    void LateUpdate()
    {
       
        // if(!isNormal)
        // {
        //     float distance = (transform.position - flock.GroupCenter).magnitude;

        //     //if this rat is within hostile radius then make it run toward the target
        //     if (distance <= hostileDist)
        //     {
        //         flock.Speed = manager.MaxSpeed * 2; //Double max speed
        //     }
        //     else if (distance <= perceptibleDist)
        //     {
        //         flock.Speed = Random.Range(manager.MinSpeed, manager.MaxSpeed);//stay in original speed range
        //     }
        //     else
        //     {
        //         flock.Speed = 0;
        //     }
        // }
        // else
        // {
        //     flock.Speed = Random.Range(manager.MinSpeed, manager.MaxSpeed);
        // }

        //flock.Speed = Random.Range(manager.MinSpeed, manager.MaxSpeed);

        //anim.SetFloat("Speed", flock.Speed); //set the animation "speed" to different animation
        //Debug.Log(flock.Speed);
    }
}
