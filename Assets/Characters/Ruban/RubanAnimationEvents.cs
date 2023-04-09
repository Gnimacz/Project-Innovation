using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubanAnimationEvents : MonoBehaviour
{
    public Rigidbody bullet;
    public Transform spawnPoint;
    public int multiplier;
    //public GameObject  laserPrefab; 
    //public Transform spwawnPoint; 
    //public int LaserForce;



    public void FireWeapon()
    {
        Rigidbody bullitInstance;
        bullitInstance =  Instantiate(bullet, spawnPoint.position, bullet.transform.rotation) as Rigidbody;
        bullitInstance.AddForce(spawnPoint.forward * multiplier);
        //Debug.Log("hello 1");
        //GameObject Laser = Instantiate(laserPrefab,spwawnPoint.position, Quaternion.identity);
       // Debug.Log("hello 2");
        //Rigidbody rb = laserPrefab.GetComponent<Rigidbody>();
       // Debug.Log("hello 3");
       // rb.velocity = (Vector3.forward*LaserForce);
    }
 
}
