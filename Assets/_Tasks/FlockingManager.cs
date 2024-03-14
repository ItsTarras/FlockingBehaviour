using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public GameObject unitPrefab;
    public int numOfunits;
    public GameObject[] allUnits;
    public Vector3 bounds = new Vector3(50, 1, 50);

    [Header("Unit Settings")]

	[Range(0.0f, 50.0f)]
	public float minSpeed; // Minimum speed of units

	[Range(0.0f, 50.0f)]
	public float maxSpeed; // Maximum speed of units

	[Range(0.0f, 50.0f)]
	public float neighbourProximity; // The distance at which a unit is regarded as a neighbor of another unit

    [Range(0.0f, 20.0f)]
	public float neighbourAllowedDistance; // The allowed distance between units

	[Range(0.0f, 20.0f)]
	public float rotationSpeed; // Turning speed of a unit. Used for avoidance steering

	public GameObject timer;
	void Start()
	{
		GenerateAllunits();
		timer.SetActive(true);
	}
	
    void GenerateAllunits(){
        // Set the size of allunits array to the total number of units.
        allUnits = new GameObject[numOfunits];

        // Spawn all available units to the scene
		for (int i = 0; i < numOfunits; i++)
		{
			//Get a random position within the bounds for the rats to spawn.
			GameObject currentEntity = unitPrefab;
			float x = transform.position.x + Random.Range(-bounds.x, (float)bounds.x);
			float y = transform.position.y + Random.Range(-bounds.y, (float)bounds.y);
			float z = transform.position.z + Random.Range(-bounds.z, (float)(bounds.z));
			currentEntity.transform.position = (new Vector3(x, y, z));

            // This array will be used in the Flock script to look for neighbours.
            allUnits[i] = Instantiate(currentEntity);


            // TODO: Set the units/ manager to be this manager
            // Since it is not possible to link the FlockManager script through a prefab,
            // you will need to link it via code. This will allow you access properties/variables between
            // the FlockingManager and Flock script.
            allUnits[i].GetComponent<Flock>().unitManager = this;
        }
    }

	
}
