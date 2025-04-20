using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] bool isCountDown;
    [SerializeField] float time;

    bool isRunning = false;
    TimeSpan timeSpan;

    // Start is called before the first frame update
    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        StopTimer();
        timerText.text = "00:00:00";
        time = 0f;
        timeSpan = TimeSpan.Zero;
    }

    // Update is called once per frame
    void Update()
    {
        
        //ignore this
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        

        if (isRunning)
        {
            if(isCountDown)
            {
                time -= Time.deltaTime;
                
            }
            else
            {
                time += Time.deltaTime;
            }

            timeSpan = TimeSpan.FromSeconds(time);

            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeSpan.Minutes,
                timeSpan.Seconds,
                (int)(time * 100) % 100); // Convert total time to centiseconds
        }
        
    }
}
