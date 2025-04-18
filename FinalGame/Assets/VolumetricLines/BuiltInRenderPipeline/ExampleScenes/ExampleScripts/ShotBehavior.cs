using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

    public float life = 3;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // get the Enemy script component on the collided object
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // subtract from enemy health
            enemy.TakeDamage(1);
        }

        // destroy the bullet after collision
        Destroy(gameObject);
    }
}
