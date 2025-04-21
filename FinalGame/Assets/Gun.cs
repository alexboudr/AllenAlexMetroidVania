using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    //[SerializeField] Transform gunAim;
    [SerializeField] LineRenderer lineRend;

    public float damage = 1f;
    public float range = 10f;
    public Camera cam;

    //sound effect stuff
    private AudioSource audioSource; //audio source...
    public AudioClip shootSound;

    public bool isGunActive = true; //need this for stupid pausijng
    public bool isVisorA = false; //need this fopr stupid visor

    void Update()
    {
        if (!isGunActive) return;
        if (isVisorA) return;

        //if(Input.GetKeyDown(KeyCode.Q))
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //initialize the audio source
    }

    void Shoot ()
    {
        //Ray rayOrigin = Camera.main.ScreenPointToRay();
        RaycastHit hit;
        if ( Physics.Raycast(cam.transform.position, cam.transform.forward, out hit) ) // , range
        {
            //Debug.Log(hit.transform.name);


            // DRAWING RAYCAST LINES FOR DEBUGGING

            //lineRend.enabled = true;
            //lineRend.SetPosition(0, bulletSpawnPoint.position);
            //lineRend.SetPosition(1, hit.point);


            //Debug.DrawRay(bulletSpawnPoint.position, hit.point - bulletSpawnPoint.position, Color.blue, 2, false);
            //Debug.DrawRay(cam.transform.position, hit.point - bulletSpawnPoint.position, Color.red, 2, false);

            Vector3 shotDirection = hit.point - bulletSpawnPoint.position;

            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(shotDirection));
            bullet.GetComponent<Rigidbody>().velocity = shotDirection.normalized * bulletSpeed;

            audioSource.PlayOneShot(shootSound, 0.7F); //play that damn sound

            //if ( hit.transform.CompareTag("Enemy") )
            //{
            //    Enemy enemy = hit.transform.GetComponent<Enemy>();
            //    enemy.TakeDamage(damage);
            //}

        }
    }
}
