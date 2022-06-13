using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float shotSpeed;
    public Rigidbody2D shot;

    public Vector2 moveDirection;

    public GameObject impactEffect;
    public GameObject windowShatterEffect;

    public int damageAmount = 1;

    // Update is called once per frame
    void Update()
    {
        shot.velocity = moveDirection * shotSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Window")
        {
            if (windowShatterEffect != null)
            {
                Instantiate(windowShatterEffect, other.transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject);
        }


        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
