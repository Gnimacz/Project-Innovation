using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
    
public class VFX_Animation_Test : MonoBehaviour
{
    [SerializeField] VisualEffect VFX;
    
   
    public void AttackVFX()
    {
      
        VFX.Play();
        Debug.Log("VFX IS WORKING");
    }
}
