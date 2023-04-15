using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attacker : MonoBehaviour
{
    public Animation animations;
  
    
    public void DoLightAttack()
    {
        animations.Play("DoLightAttack");
    }

    public void DoHeavyAttack()
    {
        animations.Play("DoHeavyAttack");
    }

    public void DoUpAttack()
    {
        animations.Play("DoUpAttack");
    }

    public void DoDownAttack()
    {
        animations.Play("DoDownAttack");
    }
    
}
