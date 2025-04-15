using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPosition : MonoBehaviour
{
    public Transform playerSpawnPosition;

    public Timer timerObj;

    // Start is called before the first frame update
    void Start()
    {
        //get player spawn position
        playerSpawnPosition.position = transform.position;
        //Debug.Log(playerSpawnPosition.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.ToString());
        if (transform.position.y <= -25f)
        {
            Debug.Log("this should activate");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //CharacterController controller = GetComponent<CharacterController>();
            //controller.enabled = false;
            //transform.position = playerSpawnPosition.position;
            //controller.enabled = true;

            //timerObj.ResetTimer();
        }
    }
}
