using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IHitable
{
    // Enemy AI variables
    private NavMeshAgent agent;
    private Rigidbody rb;

    // Patrolling / Chasing

    public Transform player;
    public float playerDistance;
    public float awareAI = 15f;
    public float AIMoveSpeed;
    //bool isChasing = false;

    [SerializeField] float waitTimeOnWaypoint = 1f;
    [SerializeField] Path path;

    
    //Animator animator;
    float time = 0f;


    // ENEMY TYPES:
    // #1: docile, patrols and doesn't attack
    // #2: medium, patrols and chases when player is near
    // #3: hostile, patrols, chases AND attacks (fires bullets) when player is near
    public int thisEnemyType = 1;


    // Attacking

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;


    // General Enemy variables

    public float enemyHealth = 3;
    public Renderer thisEnemyModel;
    private Material thisEnemyMaterial;
    //List<Material> modelMaterials;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //Animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        thisEnemyModel = GetComponent<Renderer>();

        thisEnemyMaterial = thisEnemyModel.material; //this is needed to store the current material WIHTOUT the scan visor

        //// NOTE: the way how I'm accessing the enemy model itself is VERY specific to the current testEnemy....
        //// retrieves the renderer component (aka the model) of THIS enemy (to change the model color for later)
        //thisEnemyModel = GetComponent<Renderer>().transform.GetChild(0).GetChild(2).GetComponent<Renderer>();

        //// save the original materials to switch back to, after the enemy takes damage
        //modelMaterials = thisEnemyModel.materials.ToList();

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

        if (thisEnemyType >= 2)
        {
            playerDistance = Vector3.Distance(player.position, transform.position);

            //Debug.Log(playerDistance);

            if (playerDistance < awareAI)
            {
                LookAtPlayer();

                if (playerDistance > 0.1f)
                {
                    agent.speed = AIMoveSpeed;
                    ChasePlayer();
                }
                else
                {
                    agent.destination = path.GetNextWaypoint();
                }
            }

        }
        
        if (thisEnemyType == 3)
        {
            playerDistance = Vector3.Distance(player.position, transform.position);

            if (playerDistance < awareAI)
            {
                AttackPlayer();
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

    void ChasePlayer()
    {
        transform.Translate(Vector3.forward * AIMoveSpeed * Time.deltaTime);
        agent.SetDestination(player.position);
        //agent.destination = player.position;
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            GameObject bul = Instantiate(projectile, (transform.position + transform.forward * 3f), Quaternion.identity);
            bul.tag = "EnemyBullet";
            Rigidbody rb = bul.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            //Bullet projScript = bul.GetComponent<Bullet>();
            //if (projScript != null)
            //{
            //    projScript.shooter = gameObject;
            //}

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damagePoints)
    {
        enemyHealth -= damagePoints;
        StartCoroutine(FlashRed());

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FlashRed()
    {
        thisEnemyModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        thisEnemyModel.material.color = Color.white;

        //foreach (Material mat in thisEnemyModel.materials)
        //{
        //    mat.color = Color.red;
        //}

        //yield return new WaitForSeconds(0.1f);

        //Debug.Log("modelMaterials: " + modelMaterials.Count);
        //Debug.Log("thisEnemy.materaisl: " + thisEnemyModel.materials.ToList().Count);
        //Debug.Log(thisEnemyModel.materials[1].color);

        //for (int i = 0; i < thisEnemyModel.materials.ToList().Count; i++)
        //{

        //    thisEnemyModel.materials[i] = modelMaterials[i];
        //}

        //int i = 0;
        //foreach (Material mat in thisEnemyModel.materials)
        //{
        //    mat = modelMaterials[i];
        //    i++;
        //}
    }



    public void KnockbackEntity(Transform executionSource)
    {
        Vector3 dir = (transform.position - executionSource.position).normalized;
        rb.AddForce(dir, ForceMode.Impulse);
    }

    public IEnumerator getKnockedBack(Vector3 force)
    {
        yield return null;
        agent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(force);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.velocity.magnitude < 0.05f);
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
        agent.Warp(transform.position);
        agent.enabled = true;
        yield return null;
    }


    //void OnCollisionEnter(Collision other)
    //{

    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("I just hit the player!");

    //        //gameObject.GetComponent<NavMeshAgent>();

    //        ThirdPersonController player = other.gameObject.GetComponent<ThirdPersonController>();

    //        player.Knockback();
    //    }
    //}

}
