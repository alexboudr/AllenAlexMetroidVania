using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject[] enemies;
    // Call this to spawn enemies
    public void RespawnEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true); //reactiviate!
                enemy.GetComponentInChildren<Enemy>().ResetHealth(); // Custom method to reset health or state
            }
        }
    }
}