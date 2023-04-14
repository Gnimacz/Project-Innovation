using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventTester : MonoBehaviour
{
    public SimpleServerDemo.ServerState stateToSwitchTo;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)){
            SimpleServerDemo.UpdateServerState?.Invoke(stateToSwitchTo);
        }
    }
}
