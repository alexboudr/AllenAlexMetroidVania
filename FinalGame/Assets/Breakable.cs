using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    // Start is called before the first frame update

    //sound effect stuff
    private AudioSource audioSource; //audio source...
    public AudioClip breakSound;

    private Renderer objectRenderer; //we're using this to render the material
    public Material invisibledMaterial;

    public GameObject breakeEffect;

    private bool hasShot = false;
    public int damageTakesToBreak = 2;//starting damage is at 1

    public void Break()
    {
        if (!hasShot)
        {
            hasShot = true;
            objectRenderer.material = invisibledMaterial;
            //spawn effect
            Instantiate(breakeEffect, transform.position + Vector3.up * 2.5f, transform.rotation);
            audioSource.PlayOneShot(breakSound, 1F); //play that damn sound

            Destroy(gameObject, breakSound.length);
        }
        
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
        objectRenderer = GetComponent<Renderer>();
    }


}
