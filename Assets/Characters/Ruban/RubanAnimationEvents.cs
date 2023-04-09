using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubanAnimationEvents : MonoBehaviour
{
    public GameObject  laserPrefab; 
    public Transform spwawnPoint; 
    public float LaserForce;


    public void RubanLaserbeamEvent()
    {
        GameObject Laser = Instantiate(laserPrefab,spwawnPoint.position, Quaternion.identity);
        Rigidbody rb = laserPrefab.GetComponent<Rigidbody>();
        rb.velocity = (Vector3.forward*LaserForce);
        //Destroy(laserPrefab,30f);
    }
 
}
