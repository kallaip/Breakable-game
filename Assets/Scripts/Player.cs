using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Gameplay")]
    public int initialHealth = 100;
    public int initialLives = 3;
    public int hurtVelocity = 10;
    public int looseHealthByFall = 5;
    public int fatalVelocity = 20;

    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }



   

    // Start is called before the first frame update
    void Start()
    {
      
        health = initialHealth;
      
    }

    // Update is called once per frame
    void Update()
    {
    }

  public void setPosition(Vector3 position)
    {
        transform.position = position;

    }

    private void OnCollisionEnter(Collision collision)
    {

        //Debug.Log("Player Collision");

        if(collision.relativeVelocity.magnitude > fatalVelocity)
        {
            health = 0;
            
        } else if (collision.relativeVelocity.magnitude > hurtVelocity)
        {
            int minushealth = (int)(collision.relativeVelocity.magnitude / 10) * looseHealthByFall;
            health -= minushealth;

            if(collision.collider.attachedRigidbody != null)
            {
                float mass = collision.collider.attachedRigidbody.mass;
                Debug.Log(" Mass: " + mass);
                
            }
            

            Debug.Log("Player Health decreased with " + minushealth);
        }
        Debug.Log("Player Collision Velocity: " + collision.relativeVelocity.magnitude);


    }


}
