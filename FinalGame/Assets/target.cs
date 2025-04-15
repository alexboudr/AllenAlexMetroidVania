using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    //public GameObject objectToDestroy //this is attached in inspector
    //Material hit material
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    private void OnCollisionEnter(Collision collision)
    {
        //desrtroy.gameobject(objectToDestroy);
        //change material to hit material
    }
}
