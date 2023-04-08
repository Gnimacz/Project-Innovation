using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventTester : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            SimpleServerDemo.UpdateServerState?.Invoke(SimpleServerDemo.ServerState.CharacterSelect);
        }
    }
}
