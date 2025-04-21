using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;
    private Vector3 startPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //plaery tag
        {
            SceneManager.LoadScene("PlatformLevel");
        }
    }


    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the object
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Bob up and down using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
