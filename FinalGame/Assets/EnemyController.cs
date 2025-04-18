using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float playerDistance;
    public float awareAI = 10f;
    public float AIMoveSpeed;
    //public float damping = 6.0f;

    public Transform[] navPoint;
    public UnityEngine.AI.NavMeshAgent agent;
    public int destPoint = 0;
    public Transform goal;


    // enemyTypes:

    // #1: non-hostile (does not attack)
    // #2: chaser (chases the player if they get too close)
    // #3: attacker (chases AND attacks the player if they get too close)

    public int thisEnemyType;


    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        
        agent.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if the enemy is either a Chaser or Attacker, calculate their
        // distance from the player and chase accordingly!!
        if (thisEnemyType > 1)
        {
            playerDistance = Vector3.Distance(player.position, transform.position);

            if (playerDistance < awareAI)
            {
                LookAtPlayer();
                //Debug.Log("Seen");
            }

            if (playerDistance < awareAI)
            {
                if (playerDistance > 2f)
                {
                    Chase();
                }
                else
                {
                    GoToNextPoint();
                }
            }
        }

        if(agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    void GoToNextPoint()
    {
        if (navPoint.Length == 0) { return; }

        agent.destination = navPoint[destPoint].position;
        destPoint = (destPoint + 1) % navPoint.Length;
    }

    void Chase()
    {
        transform.Translate(Vector3.forward * AIMoveSpeed * Time.deltaTime);
    }
}
