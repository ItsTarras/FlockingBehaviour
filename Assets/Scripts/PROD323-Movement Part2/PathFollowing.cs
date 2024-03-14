using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathFollowing : MonoBehaviour
{
    public PathCreator pathCreator;
    public bool cw_direction = false;
    public bool robustPred = false;
    enum PathFollowingAlg { Normal, Predictive }
    [SerializeField] PathFollowingAlg example;

    [Range(1f, 20f)]
    public float maxVelocity = 1f;
    //[Range(0.5f, 10f)]
    //public float maxAngularAcceleration = 5f;
    //[Range(1f, 10f)]
    //public float maxRotation = 5f;
    //[Range(1f, 10f)]
    //public float targetAngularRadius = 5f; // The radius for arriving at the target
    //[Range(10f, 30f)]
    //public float slowAngularRadius = 20f; // The radius for beginning to slow down
    //[Range(0.01f, 0.1f)]
    //public float timeToTargetAngular = 0.05f; // The time over which to achieve target speed
    ////The distance along the path to generate the target
    ////Can be negative if the character is moving in the reverse direction
    [Range(0.0f, 0.1f)]
    public float pathOffset = 0.03f;
    public EndOfPathInstruction end;
    [Range(0.01f, 5.0f)]
    public float predictTime = 0.1f;
    //[Range(0.01f, 1f)]
    //public float delayTime = 0.1f;

    //public ObstacleAvoidance3315 obsAvoid;

    //Vector3 target = Vector3.zero;
    float previousParam = 0f;
    //The current position on the path
    float currentParam = 0f;
    //The current position on the path
    float targetParam = 0f;
    //float targetRotation = 0f;
    //Vector3 targetAngularAcceleration = Vector3.zero;
    Vector3 target = Vector3.zero;
    //Vector3 aTarget = Vector3.zero;
    //bool isAvoiding = false;

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

        if(robustPred)
        {
            float t_param = pathCreator.path.GetClosestTimeOnPath(futurePos);
            float t_time = predictTime / 2;

            for (int i = 0; i < 10; i++)
            {
                if (cw_direction)
                {
                    if(t_param < previousParam)
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

        //transform.localRotation = Quaternion.LookRotation(v.normalized);
        //rb.rotation = Quaternion.LookRotation(v.normalized);

        rb.rotation = Quaternion.LookRotation(v.normalized);

        rb.AddForce(v.normalized * maxVelocity - rb.velocity, ForceMode.VelocityChange);
        rb.angularVelocity = Vector3.zero;
    }


}
