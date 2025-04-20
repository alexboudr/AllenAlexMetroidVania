using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    public Timer timerScript; //reference to the script
    private AudioSource audioSource;
    public AudioClip interactSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(interactSound, 0.7F); //play that damn sound
        if (other.CompareTag("Player")) //plaery tag
        {
            if (gameObject.name == "StartTrigger") 
            {
                Debug.Log("Timer Started");
                timerScript.StartTimer();
            }
            else if (gameObject.name == "EndTrigger")
            {
                Debug.Log("Timer Stopped");
                timerScript.StopTimer();
            }
        }
    }
}