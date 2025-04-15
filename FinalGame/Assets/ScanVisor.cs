using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScanVisor : MonoBehaviour
{

    public GameObject scanUI;
    public GameObject foundScannable;
    public Slider scanProgress;
    public GameObject log;
    public TMP_Text logText;
    private bool isScanning = false;
    //private bool isHitting = false;

    private ScanLog[] scanableObjects;
    public LayerMask scanLayer;

    //these are for holding down the scan button
    private float scanHoldTime = 1.5f; 
    private float scanTimer = 0f;

    void Start()
    {
        scanableObjects = FindObjectsOfType<ScanLog>(); //finds all objects with the ScanLog script attached to them
        scanUI.SetActive(false); //blue shit MUST be off
        foundScannable.SetActive(false);
        scanProgress.gameObject.SetActive(false);
        log.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Scan")) 
        {
            isScanning = !isScanning;
            scanUI.SetActive(isScanning); //show/hide UI
            foundScannable.SetActive(false);
            scanProgress.gameObject.SetActive(false);
            log.SetActive(false);
            scanTimer = 0f; //have to reset the timer in case someone decides to get out of scanning WHILE scanning those fucks

            //updates materials ofr every scannable object
            foreach (ScanLog scanLog in scanableObjects)
            {
                if (scanLog != null)
                {
                    scanLog.UpdateMaterial(isScanning);
                }
          
            }
            Time.timeScale = 1; //pauses game

        }

        if(isScanning)//have to do this or else ray cast only shoots out once
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 30f, scanLayer))//hard coded scan range
            {
                Debug.Log("I am hitting!");
                foundScannable.SetActive(true);

                if (Input.GetMouseButtonDown(0)) //down -> initial frame that button is pressed
                {
                    scanTimer = 0f; //resets timer when button is first pressed
                    scanProgress.gameObject.SetActive(true);//progress bar activates (lil sauterelle)
                }

                if (Input.GetMouseButton(0)) //no down -> button being held
                {
                    scanProgress.gameObject.SetActive(true);//progress bar activates (lil sauterelle)

                    scanTimer += Time.deltaTime;
                    scanProgress.value = scanTimer;

                    if (scanTimer >= scanHoldTime)
                    {

                        Debug.Log("I should be scanning");
                        ScanForObject(hit);
                        scanTimer = 0f;
                    }
                }

                if (Input.GetMouseButtonUp(0)) //up -> when left click is released during scan
                {
                    scanTimer = 0f; //reset timer
                    Time.timeScale = 1; //pauses game
                    scanProgress.value = 0;
                    scanProgress.gameObject.SetActive(false);//progress bar activates (lil sauterelle)
                    scanUI.SetActive(true);
                    log.SetActive(false);
                }
            }
            else
            {
                foundScannable.SetActive(false);
                scanProgress.gameObject.SetActive(false);
                log.SetActive(false);
                scanTimer = 0f;
            }
        }

        ////big if statement that checks if the  player is actually in scan visor
        //if (isScanning)
        //{
        //    if (Input.GetMouseButtonDown(0)) //down -> initial frame that button is pressed
        //    {
        //        scanTimer = 0f; //resets timer when button is first pressed
        //    }

        //    if (Input.GetMouseButton(0)) //no down -> button being held
        //    {

        //        RaycastHit hit;
        //        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * scanRange, Color.red, 1f);

        //        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, scanRange, scanLayer))
        //        {
        //            Debug.Log("I am hitting!");
        //            isHitting = !isHitting;
        //            foundScannable.SetActive(isHitting);
        //            scanTimer += Time.deltaTime;
        //            if (scanTimer >= scanHoldTime)
        //            {

        //                Debug.Log("I should be scanning");
        //                ScanForObject(hit);
        //                scanTimer = 0f;
        //            }
        //        }



        //    }

        //    if (Input.GetMouseButtonUp(0)) //up -> when left click is released during scan
        //    {
        //        scanTimer = 0f; //reset timer
        //    }
        //}
    }

    public void ScanForObject(RaycastHit hit)
    {
            ScanLog scanLog = hit.collider.GetComponent<ScanLog>();
            if (scanLog != null) //just to make doubly sure we're actually hitting something
            {
                string logToPrint = scanLog.PrintLog(); //get the log from the object

                
                if (logText != null)
                {
                    logText.text = logToPrint; //update text UI
                }
                else
                {
                    Debug.Log("no fucking text!");
                }

                log.SetActive(true);
                Time.timeScale = 0; //pauses game
                scanLog.UpdateMaterial(isScanning);
            }
        
    }
}


