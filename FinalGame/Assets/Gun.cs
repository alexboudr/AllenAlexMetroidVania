using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    //[SerializeField] Transform gunAim;
    //[SerializeField] LineRenderer lineRend;

    public float damage = 1f;
    public float range = 10f;
    public Camera cam;

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot ()
    {
        //Ray rayOrigin = Camera.main.ScreenPointToRay();
        RaycastHit hit;
        if ( Physics.Raycast(cam.transform.position, cam.transform.forward, out hit) ) // , range
        {
            //Debug.Log(hit.transform.name);


            // DRAWING RAYCAST LINES FOR DEBUGGING

            //Debug.DrawRay(bulletSpawnPoint.position, hit.point - bulletSpawnPoint.position, Color.blue, 2, false);
            //Debug.DrawRay(cam.transform.position, hit.point - bulletSpawnPoint.position, Color.red, 2, false);

            Vector3 direction = (hit.point - bulletSpawnPoint.position).normalized;

            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

            //if ( hit.transform.CompareTag("Enemy") )
            //{
            //    Enemy enemy = hit.transform.GetComponent<Enemy>();
            //    enemy.TakeDamage(damage);
            //}

        }
    }
}
