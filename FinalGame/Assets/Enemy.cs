using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float waitTimeOnWaypoint = 1f;
    [SerializeField] Path path;

    NavMeshAgent agent;
    //Animator animator;

    float time = 0f;

    float enemyHealth = 3;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        agent.destination = path.GetCurrentWaypoint();
    }

    // Update is called once per frame
    private void Update()
    {
        if(agent.remainingDistance <= 0.1f)
        {
            time += Time.deltaTime;
            if(time >= waitTimeOnWaypoint)
            {
                time = 0f;
                agent.destination = path.GetNextWaypoint();
            }
        }

        // for animation stuff
        //float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
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
