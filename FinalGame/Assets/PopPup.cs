using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//this is used for a pop up when you start the game, as well as any time you pick up a powerup

public class PopPup : MonoBehaviour
{
    private float popUpTimer = 0f;
    private float popUpCooldown = 4f;
    // Start is called before the first frame update

    public GameObject popUpUI;
    public TMP_Text popUpText;
    private bool isTimerRunning = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "FirstArea")
        {
            popUpCooldown = 7f;
            StartTimeAndText("To activate your AVI unit, press <b>Tab</b>. Hold <b>Left Mouse</b> to scan objects.");
            popUpCooldown = 5f;
        }
    }

    public void StartTimeAndText(string toPrint)
    {
        popUpUI.SetActive(true); //making it go on!
        popUpTimer = popUpCooldown; //setting the timer
        popUpText.text = toPrint;
        isTimerRunning = true;
        Debug.Log("STARTINGGGGGGGGGGGGGGGG");
    }

    void Update()
    {
        if (isTimerRunning) //this checks if the thing is actually active
        {

            popUpTimer -= Time.deltaTime;

            if (popUpTimer <= 0f)
            {
                popUpUI.SetActive(false);
                isTimerRunning = false;
                Debug.Log("shoudl be off!");
            }
        }
    }
}
