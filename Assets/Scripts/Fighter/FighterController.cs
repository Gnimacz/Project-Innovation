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
    public int playerId = 0;
    
    bool doubleJumped = false;
    Vector2 direction;
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
        
    }

    private void FixedUpdate()
    {
        //gravity
        rb.AddForce(new Vector3(0, -values.gravityPower, 0), ForceMode.Acceleration);
        
        // the player has no input while stunned
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Stunned")) return;
        
        if (IsOnFloor()) rb.velocity = new Vector3(0, rb.velocity.y, 0);
        
        if ((direction.x > 0 && rb.velocity.x < values.moveSpeed) || (direction.x < 0 && rb.velocity.x > -values.moveSpeed))
            rb.velocity += new Vector3(direction.x * values.moveSpeed * (IsOnFloor() ? 1 : values.airControlStrength), 0, 0);

    }

    public void GetHit(Vector3 from, int damage)
    {
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Stunned")) return;
        
        if (IsOnFloor())
        {
            Jump();
            doubleJumped = false;
        }
        else if (!doubleJumped)
        {
            Jump();
            doubleJumped = true;
        }
    }

    public void OnAttackButtonPressed(object sender, int id)
    {
        if (id != playerId) return;
        animator.SetTrigger("LightAttack");
        
    }

    public void OnJoystickMoved(object sender, DirectionalEventArgs input)
    {
        if (input.PlayerId != playerId) return;
        direction = input.JoystickPosition;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.LightAttack")) return;
        //we don't want to rotate the player if they are in an attack
        if(direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    bool IsOnFloor()
    {
        return Physics.OverlapSphere(feet.position, values.groundDetectionradius, LayerMask.GetMask("Platforms")).Length > 0;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, values.jumpPower, 0);
        sfx.PlayJumpSound();
    }
    
}
