using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public Animator animator;
    //public FighterManager fighterController;
    int randomPlayerId;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InputEvents.JoystickMoved += OnJoystickMoved;
        FighterManager.OnPlayerLost += OnPlayerLost;
        randomPlayerId = Random.Range(0, SimpleServerDemo.instance.clientInfoList.Count); //Initial random player id
    }

    private void OnPlayerLost(int playerId, Dictionary<GameObject, FighterController> activeFighters)
    {
        if (playerId == randomPlayerId)
        {
            randomPlayerId = Random.Range(0, activeFighters.Count);
        }
    }

    private void OnJoystickMoved(object sender, DirectionalEventArgs direction)
    {
        if (direction.PlayerId != randomPlayerId) return;
        animator.SetFloat("Direction-X", direction.JoystickPosition.x);
        animator.SetFloat("Direction-Y", direction.JoystickPosition.y);
    }
}
