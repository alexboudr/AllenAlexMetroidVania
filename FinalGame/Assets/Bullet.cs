using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;
    private AudioSource audioSource;
    public AudioClip collideSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
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

        Debug.Log("hit!");
        AudioSource.PlayClipAtPoint(collideSound, transform.position); //this plays even after its destroyed

        // destroy the bullet after collision
        Destroy(gameObject);
    }
}
