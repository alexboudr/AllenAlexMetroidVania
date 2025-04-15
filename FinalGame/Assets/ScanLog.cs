using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UIElements;

public class ScanLog : MonoBehaviour
{
    // Start is called before the first frame update
    public string logText;
    private bool isScanned = false;

    private Renderer objectRenderer; //we're using this to render the material

    public Material unscannedMaterial;
    public Material scannedMaterial;
    private Material previousMaterial; //this is so we can revert back to the base one when we get out of visor

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        previousMaterial = objectRenderer.material; //this is needed to store the current material WIHTOUT the scan visor
    }
    // Update is called once per frame

    public void UpdateMaterial(bool scanModeActive)
    {
        if (scanModeActive)
        {
            objectRenderer.material = isScanned ? scannedMaterial : unscannedMaterial; //if is scanned is true, redner using the scanned material
        }
        else
        {
            if (gameObject != null)
            {
                objectRenderer.material = previousMaterial; //restores to original material
            }
        }
    }

    public void MarkScanned()
    {
        isScanned = true;
        objectRenderer.material = scannedMaterial;
    }

    public string PrintLog()
    {
        Debug.Log(logText);

        if (!isScanned)
        {
            MarkScanned();
        }

        return logText;
    }
    //void Update()
    //{
    //    if (Input.GetButtonDown("Scan"))
    //    {
    //        if(!isScanned)
    //        {
    //            objectRenderer.material = unscannedMaterial;
    //        }
    //        else
    //        {
    //            objectRenderer.material = scannedMaterial;

                
    //        }
    //    }
    //}
}
