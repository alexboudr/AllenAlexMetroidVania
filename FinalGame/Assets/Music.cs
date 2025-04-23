using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;

    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
        PlaySong();
    }

    // Update is called once per frame
    public void PlaySong()
    {
        if (counter == 0)
        {
            audioSource.clip = song1;
        }
        else if (counter == 1)
        {
            audioSource.clip = song2;
        }
        else
        {
            audioSource.clip = song3;
        }

        audioSource.loop = true;
        audioSource.Play();
        counter++;
    }


}
