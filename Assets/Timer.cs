using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{

    public float time = 30;
    public FlockingManager AllyFlockingManager;
    public FlockingManager EnemyFlockingManager;
    private List<GameObject> EnemyFlocks = new List<GameObject>();
    private List<GameObject> AllyFlocks = new List<GameObject>();
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI EnemytextMeshPro;
    public TextMeshProUGUI AllytextMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;i < EnemyFlocks.Count;i++)
        {
            if (EnemyFlocks[i] == null)
            {
                EnemyFlocks.RemoveAt(i);
            }
        }

        for (int i = 0; i < AllyFlocks.Count; i++)
        {
            if (AllyFlocks[i] == null)
            {
                AllyFlocks.RemoveAt(i);
            }
        }


        //Update the live counter
    }
    private IEnumerator spawn()
    {
        {
            //Get the count of the number of ships, and update them LIVE.
            for (int i = 0; i < AllyFlockingManager.allUnits.Length; i++)
            {
                AllyFlocks.Add(AllyFlockingManager.allUnits[i]);
            }

            //Get the count of the number of ships, and update them LIVE.
            for (int i = 0; i < EnemyFlockingManager.allUnits.Length; i++)
            {
                EnemyFlocks.Add(EnemyFlockingManager.allUnits[i]);
            }

            yield return StartCoroutine(counter());
        }
    }

    private IEnumerator counter()
    {
        float currentTime = time;
        while (true)
        {
            if (currentTime > 0)
            {
                currentTime = time - Time.time;
                textMeshPro.text = ("Time remaining: ") + currentTime.ToString();
                AllytextMeshPro.text = AllyFlocks.Count.ToString();
                EnemytextMeshPro.text = EnemyFlocks.Count.ToString();
            } 
            else
            {
                //Check the number of enemies and allies, and count the winner.
                if (EnemyFlocks.Count > AllyFlocks.Count)
                {
                    textMeshPro.text = ("Enemies Win! You SUCK!");
                }
                else
                {
                    textMeshPro.text = ("Allies Win! You still suck!");
                }
                break;
            }
            
            //Update the textmeshpro element.
            yield return null;
        }
        
    }


}
