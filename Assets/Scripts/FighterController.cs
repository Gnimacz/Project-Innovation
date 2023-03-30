using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    public Transform feet;
    public int playerId = 0;
    public float moveSpeed = 10f;
    public float jumpPower = 10f;
    public float gravityPower = 10f;
    public float groundDetectionradius = 0.2f;

    bool doubleJumped = false;
    Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody>();
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
        
        if (IsOnFloor()) rb.velocity = new Vector3(0, rb.velocity.y, 0);
        
        if ((direction.x > 0 && rb.velocity.x < moveSpeed) || (direction.x < 0 && rb.velocity.x > -moveSpeed))
            rb.velocity += new Vector3(direction.x * moveSpeed, 0, 0);

    }

    public void OnJumpButtonPressed(object sender, int id)
    {
        if (id != playerId) return;
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
        
    }

    public void OnJoystickMoved(object sender, DirectionalEventArgs input)
    {
        if (input.PlayerId != playerId) return;
        direction = input.JoystickPosition;
    }

    bool IsOnFloor()
    {
        return Physics.OverlapSphere(feet.position, groundDetectionradius).Length > 1;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
    }
    
}
