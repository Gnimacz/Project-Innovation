using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventTester : MonoBehaviour
{
    public FightingGameServer.ServerState stateToSwitchTo;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)){
            FightingGameServer.UpdateServerState?.Invoke(stateToSwitchTo);
        }
    }
}
