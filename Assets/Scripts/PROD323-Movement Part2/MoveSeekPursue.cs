using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSeekPursue : MonoBehaviour
{

    public GameObject target;
    public bool isPursue = false;
    [Range(7f, 100f)]
    public float maxAcceleration = 12f;
    [Range(1f, 50f)]
    public float maxSpeed = 4f;
    [Range(0.1f, 20f)]
    public float targetRadius = 10f; // The radius for arriving at the target
    [Range(0.5f, 20f)]
    public float slowRadius = 5f; // The radius for beginning to slow down
    [Range(0.01f, 2f)]
    public float timeToTarget = 0.5f; // The time over which to achieve target speed
    [Range(0.1f, 5f)]
    public float maxPrediction = 1f; //The maximum prediction time

    Rigidbody rb;
    float targetSpeed = 0;
    Vector3 targetVelocity = Vector3.zero;
    Vector3 targetAcceleration = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPursue)
            SeekandArrive(target.transform.position);
        else
            PursueandArrive(target.transform.position);
    }

    void SeekandArrive(Vector3 targetPos)
    {
        // Get the vector to the target
        Vector3 v = targetPos - transform.position;
        float distance = v.magnitude;

        // Check if we are there, return no steering
        if (distance < targetRadius)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        //If we are outside the slowRadius, then move at max speed
        if (v.magnitude > slowRadius)
            targetSpeed = maxSpeed;
        else  //Otherwise calculate a scaled speed
            targetSpeed = maxSpeed * distance / slowRadius;

        //The target velocity combines speed and direction
        targetVelocity = v.normalized * targetSpeed;

        //Acceleration tries to get to the target velocity
        targetAcceleration = targetVelocity - rb.velocity;
        targetAcceleration /= timeToTarget;

        //Check if the acceleration is too fast
        if (targetAcceleration.magnitude > maxAcceleration)
        {
            targetAcceleration = targetAcceleration.normalized * maxAcceleration;
        }

        //Look at target
        rb.rotation = Quaternion.LookRotation(v.normalized);

        rb.AddForce(rb.mass * targetAcceleration);

        rb.angularVelocity = Vector3.zero;
    }

    void PursueandArrive(Vector3 targetPos)
    {
        //Check if speed gives a reasonable prediction time
        Vector3 prediction = target.GetComponent<Rigidbody>().velocity * maxPrediction;

        if (prediction.magnitude > 0.05f)
            targetPos = targetPos + prediction;

        //2. Delegate to seek
        SeekandArrive(targetPos);


    }
}
