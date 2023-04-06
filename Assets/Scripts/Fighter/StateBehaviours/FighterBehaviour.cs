using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FighterBehaviour : StateMachineBehaviour
{
    protected Animator animator;
    protected bool active;
    
    public void OnEnable()
    {
        InputEvents.AttackButtonPressed += OnAttackButtonPressed;
        InputEvents.AttackButtonPressed += OnJumpButtonPressed;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.animator is null)
            this.animator = animator;
        active = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        active = false;
    }

    protected virtual void AttackPressed()
    {
        
    }
    
    protected virtual void JumpPressed()
    {
        
    }
    
    public void OnAttackButtonPressed(object sender, int id)
    {
        if (animator is null) return;
        if (id != animator.GetInteger("PlayerId")) return;
        if (!active) return;
        AttackPressed();
    }

    public void OnJumpButtonPressed(object sender, int id)
    {
        if (animator is null) return;
        if (id != animator.GetInteger("PlayerId")) return;
        if (!active) return;
        
        JumpPressed();
    }
    
}
