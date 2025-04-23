using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public GameObject pickupEffect;
    public float rotationSpeed = 50f; 
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;
    private Vector3 startPosition;
    public string powerupName;
    public GameObject popUpManager;
    public string textToShow;
   // private AudioSource audioSource;
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();

            if (popUpManager != null)
            {
                popUpManager.GetComponent<PopPup>().StartTimeAndText(textToShow);
            }
        }
    }

    void Start()
    {
        startPosition = transform.position;
        //audioSource = GetComponent<AudioSource>(); //initialize the audio source
    }

    void Update()
    {
        //rotate the object
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Bob up and down using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void Pickup()
    {
        //spawn effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        //apply to player
        //ok this is gonna be convulted and weird, but stick with me here
        ApplySpecificPowerup(powerupName);
        
        //destroy object
        Destroy(gameObject);

    }

    private void ApplySpecificPowerup(string name)
    {
        //find the player object and get its script
        ThirdPersonController player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();

        if (player == null)
        {
            Debug.LogError("Player object not found or missing ThirdPersonController script!");
            return;
        }

        //literal huge if else statement
        if (name == "double jump")
        {
            Debug.Log("you collected the double jump");
            player.PickedupDoubleJump();
        }
        else if(name == "dash")
        {
            Debug.Log("you collected the dash");
            player.PickedupDash();
        }
        else if(name == "damage increase")
        {

            Debug.Log("You got a damage increase!");
            Music music = GameObject.FindWithTag("Player").GetComponentInChildren<Music>();
            music.PlaySong();
            Gun gun = GameObject.FindWithTag("Player").GetComponentInChildren<Gun>();
            gun.IncreaseDamage();
        }
    }
}
