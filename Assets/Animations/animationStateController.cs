using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isWalkingBHash;
    int isRunningHash;
    int isTrowingHash;
    int isJumpingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingBHash = Animator.StringToHash("isWalkingB");
        isRunningHash = Animator.StringToHash("isRunning");
        isTrowingHash = Animator.StringToHash("isTrowing");
        isJumpingHash = Animator.StringToHash("isJumping");
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalkingB = animator.GetBool(isWalkingBHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isTrowing = animator.GetBool(isTrowingHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        bool forwardPressed = Input.GetKey("d");
        bool jumpingPressed = Input.GetKey("space");
        bool backwardPressed = Input.GetKey("a");
        bool runPressed = Input.GetKey("left shift");
        bool TrowingPressed = Input.GetMouseButtonDown(0);

//-----------------[jumping]------------------------------------------
        // if play presses left click
        if (!isJumping && jumpingPressed)
        {
            // than set isWalking to true
            animator.SetBool(isJumpingHash,true);
        }

        // if play releases left click
        if (isJumping && !TrowingPressed)
        {
            // than set isWalking to false
            animator.SetBool(isJumpingHash,false);
        }
//-----------------[Trowing]------------------------------------------
        // if play presses left click
        if (!isTrowing && TrowingPressed)
        {
            // than set isWalking to true
            animator.SetBool(isTrowingHash,true);
        }

        // if play releases left click
        if (isTrowing && !TrowingPressed)
        {
            // than set isWalking to false
            animator.SetBool(isTrowingHash,false);
        }
//-----------------[walking backwards]------------------------------------------
        // if play presses w
        if (!isWalkingB && backwardPressed)
        {
            // than set isWalking to true
            animator.SetBool(isWalkingBHash,true);
        }

        // if play releases w
        if (isWalkingB && !backwardPressed)
        {
            // than set isWalking to false
            animator.SetBool(isWalkingBHash,false);
        }

//-----------------[walking forward]------------------------------------------
        // if play presses w
        if (!isWalking && forwardPressed)
        {
            // than set isWalking to true
            animator.SetBool(isWalkingHash,true);
        }

        // if play releases w
        if (isWalking && !forwardPressed)
        {
            // than set isWalking to false
            animator.SetBool(isWalkingHash,false);
        }

//-----------------[RUNNING]------------------------------------------
        // if player is pressing w and shift
        if (!isRunning && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }

        // if player stopped running

        if (isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }

    }
}
