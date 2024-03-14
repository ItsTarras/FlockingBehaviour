using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathandSeparation : MonoBehaviour
{
        public PathCreator pathCreator;
    public bool cw_direction = false;
    public bool robustPred = false;
    enum PathFollowingAlg { Normal, Predictive }
    [SerializeField] PathFollowingAlg example;

    [Range(1f, 20f)]
    public float maxVelocity = 1f;
    [Range(0.0f, 0.1f)]
    public float pathOffset = 0.03f;
    public EndOfPathInstruction end;
    [Range(0.01f, 5.0f)]
    public float predictTime = 0.1f;
    [Range(0.1f, 10f)]
    public float threshold = 1f; //The threshold to take action
    [Range(1f, 100f)]
    public float decayCoefficient = 10f;
    public GameObject[] targetList;

    float previousParam = 0f;
    //The current position on the path
    float currentParam = 0f;
    //The current position on the path
    float targetParam = 0f;
    Vector3 target = Vector3.zero;


    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        switch (example)
        {
            case PathFollowingAlg.Normal:
                GetSteering();
                break;
            case PathFollowingAlg.Predictive:
                GetSteeringPredictive();
                break;
        }
    }

    void GetSteering()
    {
        //1. Calculate the target to delegate to face
        //Find the current position on the path
        currentParam = pathCreator.path.GetClosestTimeOnPath(transform.position);

        //Offset it
        targetParam = cw_direction ? currentParam - pathOffset : currentParam + pathOffset;

        //Get the target position
        target = pathCreator.path.GetPointAtTime(targetParam, end);

        //2. Delegate to seek
        Seek(target);

    }

    void GetSteeringPredictive()
    {
        //1. Calculate the target to delegate to face
        //Find the predicted future location.
        Vector3 futurePos = transform.position + rb.velocity * predictTime;
        //Find the current position on the path

        if (robustPred)
        {
            float t_param = pathCreator.path.GetClosestTimeOnPath(futurePos);
            float t_time = predictTime / 2;

            for (int i = 0; i < 10; i++)
            {
                if (cw_direction)
                {
                    if (t_param < previousParam)
                        break;
                }
                else
                {
                    if (t_param > previousParam)
                        break;
                }
                futurePos = transform.position + rb.velocity * t_time;
                t_param = pathCreator.path.GetClosestTimeOnPath(futurePos);
                t_time = t_time / 2;
            }

            previousParam = currentParam;
            currentParam = t_param;
        }
        else
        {
            currentParam = pathCreator.path.GetClosestTimeOnPath(futurePos);
        }


        //Offset it
        targetParam = cw_direction ? currentParam - pathOffset : currentParam + pathOffset;

        //Get the target position
        Vector3 target = pathCreator.path.GetPointAtTime(targetParam, end);

        //2. Delegate to seek
        Seek(target);
    }

    void Seek(Vector3 targetPos)
    {
        Vector3 v;
        v = targetPos - transform.position;

        //Collision avoidance algorithm
        for (int i = 0; i < targetList.Length; i++)
        {
            if (!GameObject.ReferenceEquals(this, targetList[i])) //not this gameobject
            {
                //Check if the target is close
                Vector3 direction = transform.position - targetList[i].transform.position;
                float distance = direction.magnitude;
                //direction = Quaternion.Euler(0f, 90f, 0f) * direction;
                direction = Vector3.Cross(direction.normalized, Vector3.up); ;

                if (distance < threshold)
                {
                    //Calculate the strength of repulsion (here using the inverse square law)
                    float strength = Mathf.Min(decayCoefficient / (distance * distance), maxVelocity);

                    //Add the acceleration
                    rb.AddForce(strength * direction.normalized - rb.velocity, ForceMode.VelocityChange);
                    Debug.DrawLine(transform.position, transform.position + strength * direction.normalized, Color.red);
                    //Debug.Log(gameObject.name + "  " + direction);
                }
            }
        }

        rb.rotation = Quaternion.LookRotation(v.normalized);

        rb.AddForce(v.normalized * maxVelocity - rb.velocity, ForceMode.VelocityChange);
        rb.angularVelocity = Vector3.zero;
    }
}
