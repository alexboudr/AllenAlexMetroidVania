using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseUI;
    bool isActive = false; // to control the pause UI

    public GameObject areYouSure; //this is a confirmation panel that either goes to main menu or exits the game entirely

    private int selectedOption = 0;

    //disable gun script when paused
    public Gun scriptToDisable;
    public ScanVisor visor;


    private void Toggle()
    {
        if (scriptToDisable != null)
        {
            scriptToDisable.isGunActive = !scriptToDisable.isGunActive;
        }

        if (visor != null)
        {
            visor.poop = !visor.poop;
        }
    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseUI.SetActive(isActive);
        areYouSure.SetActive(isActive);

        //mouse stuff
        LockMouse();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isActive = !isActive;
            
            Debug.Log("This is getting activated");


            if (isActive)
            {
                UnlockMouse();
            }
            else
            {
                LockMouse();
            }

            Toggle();

            pauseUI.SetActive(isActive);
            areYouSure.SetActive(false);

            if (isActive)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void Resume()
    {
        //on click, resume game
        isActive = false;
        LockMouse();
        pauseUI.SetActive(isActive);

        Time.timeScale = 1f;//resume game time
        
        Toggle();
    }

    //this method SUCKS
    public void AreYouSureMenu(int code) 
    {
        areYouSure.SetActive(isActive); //activates are you sure panel lol
        selectedOption = code;
    }

    public void ExitTo(int yesNo) //code is either 0: for no, 1: main menu, 2: exit application
    {
        if (yesNo == 0) //selected no, go back to previous menu
        {
            areYouSure.SetActive(!isActive);
        }
        else
        {
            if (selectedOption == 1)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
            else if (selectedOption == 2)
            {
                Debug.Log("Player quit!!!! what the FUCK?");
                Application.Quit();
            }
            else
            {
                Debug.Log("this uhhhhhhh... shouldn't be activating");
            }
        }
        //if (selectedOption == 0)
        //{
        //    areYouSure.SetActive(!isActive);
        //}
        //else if (selectedOption == 1) 
        //{
        //    SceneManager.LoadScene("MainMenu");
        //}
        //else if(selectedOption == 2)
        //{
        //    Debug.Log("Player quit!!!! what the FUCK?");
        //    Application.Quit();
        //}
        //else
        //{
        //    Debug.Log("this uhhhhhhh... shouldn't be activating");
        //}
    }
}
