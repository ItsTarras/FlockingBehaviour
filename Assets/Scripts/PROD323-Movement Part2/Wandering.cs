using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour
{

    public GameObject target;

    [Header("Perception")]
    [Range(10.0f, 20.0f)]
    [SerializeField] float perceptibleDist; //Distance that the rat detects the target character
    [Range(0.0f, 10.0f)]
    [SerializeField] float hostileDist; // Distance that the rat becomes aggressive and run toward the target
    
    [Header("Movement")]
    //The maximum acceleration of the character
    [Range(0f, 10f)]
    public float minSpeed = 5f;
    [Range(0f, 20f)]
    public float maxSpeed = 5f;
    [Range(0f, 5f)]
    public float wanderRadius = 0.5f;
    [Range(0f, 5f)]
    public float wanderTimer = 2f;
    [SerializeField] AnimationCurve distribution;

    Animator anim;
    Rigidbody rb;
    float speed;
    float timer = 0;
    bool isWandering = true;
    bool isDirected = false;
    Vector3 direction = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Wander();
    }
    void LateUpdate()
    {
        float distance = (transform.position - target.transform.position).magnitude;

        //if this rat is within hostile radius then make it run toward the target
        if (distance <= hostileDist)
        {
            speed = maxSpeed; //Double max speed
            isWandering = false;
        }
        else if (distance <= perceptibleDist)
        {
            speed = minSpeed*2;//stay in original speed range
            isWandering = false;
        }
        else
        {
            if (!isWandering)
            {
                timer = 0;
                isWandering = true;
            }

            if (timer <= 0)
            {
                speed = Random.value > 0.5f ? minSpeed : 0;
                timer = Random.Range(wanderTimer, wanderTimer * 2);
                isDirected = false;
            }
        }
        anim.SetFloat("Speed", speed); //set the animation "speed" to different animation
        //Debug.Log(flock.Speed);
    }

    void Wander()
    {

        if (isWandering)
        {
            if (speed > 0 && !isDirected)
            {
                float p = distribution.Evaluate(Random.value);
                Vector3 v = Vector3.Cross(transform.forward, Vector3.up); //vector sideway
                v = p * wanderRadius * v; // Scale by wanderRadius in left or right direction
                direction = transform.forward * 5f + v;
                isDirected = true;
            }

            timer -= Time.fixedDeltaTime;
        }
        else
        {
            Vector3 v1 = transform.position - target.transform.position; //vector of target and rat

            float p = distribution.Evaluate(Random.value);
            Vector3 v2 = Vector3.Cross(v1.normalized, Vector3.up); //vector sideway
            v2 = p * wanderRadius * v2; // Scale by wanderRadius in left or right direction

            Vector3 v3 = transform.forward;

            direction = v1 + v2 + v3;
        }

        if(direction.magnitude > 0)
        {
            if (speed > 0)
                transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(direction.normalized, Vector3.up), transform.rotation, Time.fixedDeltaTime);

            //rb.velocity = direction.normalized * speed * 2;
            rb.AddForce(direction.normalized * speed * 2 - rb.velocity, ForceMode.VelocityChange);
        }

    }

}
