using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject objectToDestroy; //this is attached in inspector
    private Renderer objectRenderer; //we're using this to render the material

    public Material unhitMaterial;
    public Material hitMaterial;

    //audio stuff
    private AudioSource audioSource;
    public AudioClip activatedSound;

    bool hasBennHit = false;
    //Material hit material
    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material = unhitMaterial;
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
    }
    public void hit()
    {
        objectRenderer.material = hitMaterial;
        if (!hasBennHit)
        {
            audioSource.PlayOneShot(activatedSound, 0.7F); //play that damn sound
            hasBennHit = !hasBennHit;
        }
        
        Destroy(objectToDestroy);
    }
}
