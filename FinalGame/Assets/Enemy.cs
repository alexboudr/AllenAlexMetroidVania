using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float playerDistance;
    public float awareAI = 15f;
    public float AIMoveSpeed;
    //bool isChasing = false;


    [SerializeField] float waitTimeOnWaypoint = 1f;
    [SerializeField] Path path;

    NavMeshAgent agent;
    //Animator animator;

    float time = 0f;

    float enemyHealth = 3;

    public int thisEnemyType;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //Animator = GetComponent<Animator>();

        agent.speed = 3.5f;
    }

    // Start is called before the first frame update
    private void Start()
    {
        agent.destination = path.GetCurrentWaypoint();
        agent.autoBraking = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // if the enemy is either a Chaser or Attacker, calculate their
        // distance from the player and chase accordingly!!

        if (thisEnemyType > 1)
        {
            playerDistance = Vector3.Distance(player.position, transform.position);

            //Debug.Log(playerDistance);

            if (playerDistance < awareAI)
            {
                LookAtPlayer();

                if (playerDistance > 0.1f)
                {
                    agent.speed = AIMoveSpeed;
                    Chase();
                }
                else
                {
                    agent.destination = path.GetNextWaypoint();
                }
            }

        }

        if (agent.remainingDistance < 0.1f)
        {
            time += Time.deltaTime;
            if (time >= waitTimeOnWaypoint)
            {
                time = 0f;
                agent.destination = path.GetNextWaypoint();
            }
        }


        // for animation stuff
        //float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
    }

    void LookAtPlayer()
    {
        transform.LookAt(player);
    }

    void Chase()
    {
        transform.Translate(Vector3.forward * AIMoveSpeed * Time.deltaTime);
        agent.SetDestination(player.position);
        //agent.destination = player.position;
    }

    public void TakeDamage(float damagePoints)
    {
        enemyHealth -= damagePoints;

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
