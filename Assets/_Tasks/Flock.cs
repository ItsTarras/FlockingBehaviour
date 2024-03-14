using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class Flock : MonoBehaviour
{
    public FlockingManager unitManager;
    float unitSpeed; // Speed of the unit

    Vector3 avgAvoid; // Roughly the direction the unit has to avoid its neighbours
    Vector3 avgCenter; // Roughly the average position of the flock
    float avgSpeed; // Average speed of the flock. Units within the flock cannot move way too fast or way too slow.
    float distance; // Use to determine the distance between the other unit and this unit
    int clusterSize; // Records the number of units that are considered neighbours by this unit

    void Start()
    {
        // Generate a random speed from the settings from the Flocking Manager
        unitSpeed = Random.Range(unitManager.minSpeed, unitManager.maxSpeed);
    }

    void Update()
    {
        ApplyBlendedSteeringBehaviours();

        // Move unit towards its forward vector
        transform.Translate(0, 0, Time.deltaTime * unitSpeed);
    }

    void ApplyBlendedSteeringBehaviours()
    {
        // Setting default values
        avgAvoid = Vector3.zero;
        avgCenter = Vector3.zero;
        avgSpeed = 0.01f;
        distance = 0;
        clusterSize = 0;

        // Iterate through all the members in the flock and check for neighbours that are too close then try to avoid
        foreach (GameObject unit in unitManager.allUnits)
        {
            // No point checking if you unit is neighbouring with itself
            if (unit != this.gameObject)
            {
                // TODO: Calculate distance between this unit and the next unit
                distance = Vector3.Distance(unit.transform.position, transform.position);

                // Check if the distance between the two units are within the neighbour proximity limits
                // If within the limits, then start apply steering/flocking rules
                // COHESION - Add the unit's position to the avgCenter and increase the cluster size by 1
                // SEPARATION - If the distance is within the neighbour allowed distance limits,
                // then add the direction towards the current unit from the next unit to avgAvoid
                // ALIGNMENT - Add the unit's speed to avgSpeed

                if (distance <= unitManager.neighbourProximity)
                {
                    //Cohesion
                    avgCenter += unit.transform.position;
                    clusterSize++;
                   
                    //SEPARATION
                    if (distance < unitManager.neighbourAllowedDistance)
                    {
                        Vector3 direction = transform.position - unit.transform.position;
                        avgAvoid += direction;
                    }


                    // TODO: ALIGNMENT
                    avgSpeed += unitSpeed;
                }
            }
        }

        // If there are neighbors, then prepare to turn away from them
        if (clusterSize > 0)
        {
            // TODO: Update the values based on the cluster size
            // avgCenter is the average center of the flock divided by the cluster size
            // unitspeed is the average speed of the flock divided by the cluster size
            
            avgCenter = avgCenter / clusterSize;
            unitSpeed = avgSpeed / clusterSize;
            
            // TODO: Set the unit's new direction based on the sum of the avgCenter and avgAvoid
            Vector3 direction = (avgCenter + avgAvoid) - transform.position;


            // If direction is not zero, then unit will need to change heading to avoid collision with the other unit
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), unitManager.rotationSpeed * Time.deltaTime);
        }
    }
}
