using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isWalkingBHash;
    int isRunningHash;
    int isPunchingHash;
    int isJumpingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingBHash = Animator.StringToHash("isWalkingB");
        isRunningHash = Animator.StringToHash("isRunning");
        isPunchingHash = Animator.StringToHash("isPunching");
        isJumpingHash = Animator.StringToHash("isJumping");
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalkingB = animator.GetBool(isWalkingBHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isPunching = animator.GetBool(isPunchingHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        bool forwardPressed = Input.GetKey("d");
        bool jumpingPressed = Input.GetKey("space");
        bool backwardPressed = Input.GetKey("a");
        bool runPressed = Input.GetKey("left shift");
        bool PunchingPressed = Input.GetMouseButtonDown(0);

//-----------------[jumping]------------------------------------------
        // if play presses left click
        if (!isJumping && jumpingPressed)
        {
            // than set isWalking to true
            animator.SetBool(isJumpingHash,true);
        }

        // if play releases left click
        if (isJumping && !PunchingPressed)
        {
            // than set isWalking to false
            animator.SetBool(isJumpingHash,false);
        }
//-----------------[Punching]------------------------------------------
        // if player presses left click
        if (!isPunching && PunchingPressed)
        {
            // than set ispunching to true
            animator.SetBool(isPunchingHash,true);
        }

        // if play releases left click
        if (isPunching && !PunchingPressed)
        {
            // than set ispunching to false
            animator.SetBool(isPunchingHash,false);
        }
//-----------------[walking backwards]------------------------------------------
        // if play presses a
        if (!isWalkingB && backwardPressed)
        {
            // than set isWalkingB to true
            animator.SetBool(isWalkingBHash,true);
        }

        // if play releases a
        if (isWalkingB && !backwardPressed)
        {
            // than set isWalkingb to false
            animator.SetBool(isWalkingBHash,false);
        }

//-----------------[walking forward]------------------------------------------
        // if play presses d
        if (!isWalking && forwardPressed)
        {
            // than set isWalking to true
            animator.SetBool(isWalkingHash,true);
        }

        // if play releases d
        if (isWalking && !forwardPressed)
        {
            // than set isWalking to false
            animator.SetBool(isWalkingHash,false);
        }

//-----------------[RUNNING]------------------------------------------
        // if player is pressing d and shift
        if (!isRunning && (forwardPressed && runPressed))
        {
            //than set running forward to true
            animator.SetBool(isRunningHash, true);
        }

        // if player stopped running

        if (isRunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }

    }
}
