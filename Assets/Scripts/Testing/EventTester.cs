using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)){
            FindObjectOfType<FighterController>().GetHit(Vector3.zero, 1);
        }
            
    }
}
