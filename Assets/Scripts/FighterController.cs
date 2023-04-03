using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    SfxPlayer sfx;
    public Transform feet;
    public int playerId = 0;
    public float moveSpeed = 10f;
    public float airControlStrength = 0.2f;
    public float jumpPower = 10f;
    public float gravityPower = 10f;
    public float groundDetectionradius = 0.2f;
    public float upwardKnockbackMultiplier = 0.5f;
    public float knockbackPower = 10f;
    
    bool doubleJumped = false;
    Vector2 direction;
    int health = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
        sfx = transform.GetComponent<SfxPlayer>();
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
        rb.AddForce(new Vector3(0, -gravityPower, 0), ForceMode.Acceleration);
        
        // the player has no input while stunned
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Stunned")) return;
        
        if (IsOnFloor()) rb.velocity = new Vector3(0, rb.velocity.y, 0);
        
        if ((direction.x > 0 && rb.velocity.x < moveSpeed) || (direction.x < 0 && rb.velocity.x > -moveSpeed))
            rb.velocity += new Vector3(direction.x * moveSpeed * (IsOnFloor() ? 1 : airControlStrength), 0, 0);

    }

    public void GetHit(Vector3 from, int damage)
    {
        health += damage;
        sfx.PlayPunchSound();

        Vector3 knockbackDirection = (transform.position - from).normalized;
        Vector3 knockback = knockbackDirection * knockbackPower * (health == 0 ? 0 : health / 100f);
        rb.AddForce(knockback + Vector3.up * knockback.magnitude * upwardKnockbackMultiplier, ForceMode.Impulse);
        
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
        return Physics.OverlapSphere(feet.position, groundDetectionradius, LayerMask.GetMask("Platforms")).Length > 0;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
    }
    
}
