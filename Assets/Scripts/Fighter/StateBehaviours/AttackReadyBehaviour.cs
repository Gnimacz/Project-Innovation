using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AttackReadyBehaviour : StateMachineBehaviour
{
    Animator animator;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
    }
    
    
    
    void LightAttack()
    {
        animator.SetTrigger("LightAttack");
    }

    void LongAttack()
    {
        
    }
    
    public void OnAttackButtonPressed(object sender, int id)
    {
        if (id != animator.GetInteger("PlayerId")) return;
        animator.SetTrigger("LightAttack");
        
    }
}
