using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [Header("Explosion")]

    public float breakAtVelocity;
    public float explosionRadius;
    public float explosionPower;
    public float explosionUpwardsPower;
    public Transform brokenPlatform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakAtVelocity)
        {
            //Debug.Log("break!!!---->" + collision.relativeVelocity.magnitude);
            if (brokenPlatform)
            {
                Destroy(gameObject);
                brokenPlatform = Instantiate(brokenPlatform, transform.position, transform.rotation);
            }


                //doExplosion(collision.relativeVelocity.magnitude);

        }
        
    }



    public void doExplosion(float magnitude)
    {
        if (brokenPlatform)
        {
            Destroy(gameObject);
            brokenPlatform = Instantiate(brokenPlatform, transform.position, transform.rotation);
        }

        Vector3 explodePosition = brokenPlatform.position;
        Collider[] colliders = Physics.OverlapSphere(explodePosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.attachedRigidbody)
            {
                hit.attachedRigidbody.AddExplosionForce(explosionPower * magnitude,
                    explodePosition, explosionRadius, explosionUpwardsPower);
            }
        }
       
    }
}
