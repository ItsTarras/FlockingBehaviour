using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovement : MonoBehaviour
{
   [Range(20f, 200f)]
    public float maxDist = 30f;

    [Range(0.1f, 4f)]
    public float maxPrediction = 2f;

    [Range(0f, 100f)]
    public float maxAcceleration = 0.2f;

    [Range(1f, 20f)]    
    public float timetoTarget = 5f;

    private GameObject player;
    private Rigidbody rb;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();      
    }

    // Update is called once per frame
    void Update()
    {
          GetSteeringPursue();      
    }

    void GetSteeringPursue()
    {
       Vector3 direction;
        float prediction;
        //1. Calculate the target to delegate to seek & Work out the distance to target
        direction = player.transform.position - transform.position;

        float distance = direction.magnitude;

        if(distance < maxDist) {
            rb.velocity = Vector3.zero;
            return;
        }  
     
        //Work out our current speed
        float speed = rb.velocity.magnitude;

        //Check if speed gives a reasonable prediction time
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else //Otherwise calculate the prediction time
            prediction = distance / speed;

        //Put the target together
        Vector3 targetPos = player.transform.position;
        targetPos += player.GetComponent<Rigidbody>().velocity * prediction;

        //2. Delegate to seek
        Seek(targetPos);
            
        //3. Face
        //if(rb.velocity.magnitude > 0)
        //    FaceTarget();

    }

    void Seek(Vector3 targetPos)
    {
        Vector3 v = targetPos - transform.position; /*Seek*/
        rb.AddForce(v.normalized * maxAcceleration, ForceMode.Acceleration);
    }

    void FaceTarget()
    {
        transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up), transform.rotation, Time.deltaTime/timetoTarget);
    }
}
