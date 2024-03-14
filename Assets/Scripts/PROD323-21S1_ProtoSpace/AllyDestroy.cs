using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyDestroy : MonoBehaviour
{
    public GameObject explosion;
    public Slider healthbar;
    public int health;

    private Rigidbody shipRB;
    private Vector3 contactPos;
    private Quaternion contactRot;
    
    // Start is called before the first frame update
    void Start()
    {
        shipRB = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 0)
        {
            Instantiate(explosion, contactPos, contactRot);
            Destroy(gameObject, 0.1f);
        }      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "LaserEnemy")
        {
            health -= 1;
            healthbar.value = health;
            ContactPoint contact = collision.contacts[0];
            contactRot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            contactPos = contact.point;
            Destroy(collision.gameObject);
        }
    }

}
