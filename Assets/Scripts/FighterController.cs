using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public int playerId = 0;
    public bool jumpPressed = false;
    public bool attackPressed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        InputEvents.JumpButtonPressed += OnJumpButtonPressed;
        InputEvents.AttackButtonPressed += OnAttackButtonPressed;
        InputEvents.JoystickMoved += OnJoystickMoved;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnJumpButtonPressed(object sender, int id)
    {
        if (id == playerId)
        {
            transform.position += Vector3.up * 20 * Time.deltaTime;
        }
    }

    public void OnAttackButtonPressed(object sender, int id)
    {
        if (id == playerId)
        {
            transform.position += Vector3.right * 20 * Time.deltaTime;
        }
    }

    public void OnJoystickMoved(object sender, DirectionalEventArgs args)
    {
        if (args.PlayerId == playerId)
        {
            transform.Translate(new Vector3(args.JoystickPosition.x,args.JoystickPosition.y,0) * 20 * Time.deltaTime);
        }
    }
    
}
