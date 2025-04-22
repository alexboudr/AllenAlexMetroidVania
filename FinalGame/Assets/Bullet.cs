using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damageDealt = 3;

    //public GameObject shooter;

    private AudioSource audioSource;
    public AudioClip collideSound;
    public GameObject collideEffect;

    //private Collider cd;

    //this not only plays the sound, it also plays using spatial audio!! (so the further you are from the bullet, the less noise it'll make)
    void PlayCollisionSound()
    {
        //temporary object created at the bullets position
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = transform.position;

        //attach an audio srouce! so the soun dcan actually play
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = collideSound;

        //spatial properties
        audioSource.spatialBlend = 1f;         //0 = 2D, 1 = 3D
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 500f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        audioSource.Play();

        //destroy the temp object once the sound finishes
        Destroy(tempGO, collideSound.length);
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
        Destroy(gameObject, damageDealt);
    }

    void OnCollisionEnter(Collision collision)
    {
        //IHitable hitable = collision.gameObject.GetComponent<IHitable>();

        //if (hitable != null && collision.gameObject != shooter)
        //{
        //    hitable.TakeDamage(damageDealt);
        //}

        //if(collision.gameObject.CompareTag("Player"))
        //{
        //    Physics.IgnoreCollision(collision.collider);
        //}

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // get the Enemy script component on the collided object
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // subtract from enemy health
            enemy.TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Target"))
        {
            //get target script
            Target target = collision.gameObject.GetComponent<Target>();

            //do da thang
            target.hit();
        }
        else if (collision.gameObject.CompareTag("Breakable"))
        {
            Breakable obstacle = collision.gameObject.GetComponent<Breakable>();

            obstacle.Break();
        }

        Debug.Log("hit!");
        //spawn effect
        GameObject vfx = Instantiate(collideEffect, transform.position, transform.rotation);
        PlayCollisionSound();

        // destroy the bullet after collision

        Destroy(vfx, 1.5f);
        Destroy(gameObject);
    }
}


