using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualEffects;

public class Attacker : MonoBehaviour
{
    public Animation animations;
    public VisualEffect VFX_Hit;

    public void DoVfx()
    {
        VFX_Hiz.Play();
    }

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
