using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    public Timer timerScript; //reference to the script

    private void OnTriggerEnter(Collider other)
    {
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