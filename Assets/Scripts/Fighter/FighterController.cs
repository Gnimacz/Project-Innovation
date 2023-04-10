using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    SfxPlayer sfx;
    FighterValues values;
    public Transform feet;
    [NonSerialized]
    public int playerId;
    
    bool usedDoubleJump = false;
    bool usedUpAttack = false;
    Vector2 joyInput;
    DirectionalEventArgs.JoystickAngle inputDirection;
    int health = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
        sfx = transform.GetComponent<SfxPlayer>();
        values = transform.GetComponent<FighterValues>();
        InputEvents.JumpButtonPressed += OnJumpButtonPressed;
        InputEvents.AttackButtonPressed += OnAttackButtonPressed;
        InputEvents.JoystickMoved += OnJoystickMoved;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsOnGround", IsOnFloor());
        animator.SetFloat("InputX", Mathf.Abs(joyInput.x), 0, 1);
        animator.SetFloat("JumpTransition", Mathf.Clamp(rb.velocity.y, -1, 1));
    }

    private void FixedUpdate()
    {
        //gravity
        rb.AddForce(new Vector3(0, -values.gravityPower, 0), ForceMode.Acceleration);

        if (IsOnFloor() && !animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking.UpAttack"))
            usedUpAttack = false;
        
        // the player has no input while stunned or blocking
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Movement.Stunned"))
        {
            //overrider horizontal velocity when on ground so player doesn't slide
            if (IsOnFloor()) rb.velocity = new Vector3(0, rb.velocity.y, 0);

            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking.UpAttack"))
            {
                //dash up with up attack
                rb.velocity = new Vector3(0, values.upAttackSpeed, 0);
            }
            else
            {
                //no moving while blocking
                if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking.Block"))
                {
                    //player input
                    if ((joyInput.x > 0 && rb.velocity.x < values.moveSpeed) || (joyInput.x < 0 && rb.velocity.x > -values.moveSpeed))
                        rb.velocity += new Vector3(joyInput.x * values.moveSpeed * (IsOnFloor() ? 1 : values.airControlStrength), 0, 0);
                }
            }
            
        }
        
    }

    public void GetHit(Vector3 from, int damage)
    {
        //don't get hit while blovking
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking.Block")) return;
        
        health += damage;
        sfx.PlayPunchSound();

        Vector3 knockbackDirection = (transform.position - from).normalized;
        Vector3 knockback = knockbackDirection * values.knockbackPower * (health == 0 ? 0 : health / 100f);
        rb.AddForce(knockback + Vector3.up * knockback.magnitude * values.upwardKnockbackMultiplier, ForceMode.Impulse);
        
        animator.SetTrigger("Stunned");
    }
    
    public void OnJumpButtonPressed(object sender, int id)
    {
        if (id != playerId) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Movement.Stunned")) return; // no jumping while stunned
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking.Ready")) return; // no jumping while attacking
        
        if (IsOnFloor())
        {
            Jump();
            usedDoubleJump = false;
        }
        else if (!usedDoubleJump)
        {
            Jump();
            usedDoubleJump = true;
        }
    }
    
    public void OnAttackButtonPressed(object sender, int id)
    {
        if (id != playerId) return;
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking.Ready")) return;

        if (inputDirection == DirectionalEventArgs.JoystickAngle.Neutral)
            animator.SetTrigger("LightAttack");
        
        if (inputDirection == DirectionalEventArgs.JoystickAngle.Left || inputDirection == DirectionalEventArgs.JoystickAngle.Right)
            animator.SetTrigger("HeavyAttack");
        
        if (inputDirection == DirectionalEventArgs.JoystickAngle.Down && IsOnFloor())
            animator.SetTrigger("Block");
        
        if (inputDirection == DirectionalEventArgs.JoystickAngle.Down && !IsOnFloor())
            animator.SetTrigger("DownAttack");

        if (inputDirection == DirectionalEventArgs.JoystickAngle.Up && !usedUpAttack)
        {
            animator.SetTrigger("UpAttack");
            usedUpAttack = true;
        }
        
    }
    
    public void OnJoystickMoved(object sender, DirectionalEventArgs inputArgs)
    {
        if (inputArgs.PlayerId != playerId) return;
        joyInput = inputArgs.JoystickPosition;
        inputDirection = inputArgs.JoystickDirection;
        //Null check for the animator
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking.Ready")) return;
        //we don't want to rotate the player if they are in an attack
        if(joyInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(joyInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    bool IsOnFloor()
    {
        bool touchingGround =  Physics.OverlapSphere(feet.position, values.groundDetectionradius, LayerMask.GetMask("Platforms")).Length > 0;
        if (touchingGround && rb.velocity.y <= 0) return true;
        return false;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, values.jumpPower, 0);
        sfx.PlayJumpSound();
    }
    
}
