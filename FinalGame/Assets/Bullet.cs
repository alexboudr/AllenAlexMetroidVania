using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damageDealt = 3;

    //private GameObject shooter;

    private AudioSource audioSource;
    public AudioClip collideSound;
    public GameObject collideEffect;

    public int bulletDamage;

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
        //shooter = GetComponent<GameObject>();
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
        Destroy(gameObject, damageDealt);
    }

    void OnCollisionEnter(Collision other)
    {
        //IHitable hitable = collision.gameObject.GetComponent<IHitable>();


        if (gameObject.CompareTag("Bullet") && other.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.gameObject.GetComponent<Collider>());
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            // get the Enemy script component on the collided object
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // subtract from enemy health
            enemy.TakeDamage(bulletDamage);
            //enemy.Execute(transform);
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            //get target script
            Target target = other.gameObject.GetComponent<Target>();

            //do da thang
            target.hit();
        }
        else if (other.gameObject.CompareTag("Breakable"))
        {
            //check damage!!!!

            Breakable obstacle = other.gameObject.GetComponent<Breakable>();

            if (obstacle.damageTakesToBreak <= bulletDamage)
            {
                
                obstacle.Break();
            }
        
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


