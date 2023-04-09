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
        GameObject Laser = Instantiate(laserPrefab,spwawnPoint.position, spwawnPoint.rotation);
        Rigidbody rb = laserPrefab.GetComponent<Rigidbody>();
        rb.AddForce(spwawnPoint.forward * LaserForce, ForceMode.Impulse);
    }
}
