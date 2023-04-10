using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControlTest : MonoBehaviour
{
    public string RemoteInput;

    Vector3 velocity = Vector3.zero;
    public float speed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        FightingGameServer.OnMessageReceived += /**/(string message) => {
            // RemoteInput = message;
            ParseInput(message);
        };
        /**/
    }

    // Update is called once per frame
    void Update()
    {
        // ParseInput(RemoteInput);
        // transform.Translate(velocity * speed * Time.deltaTime);
    }

    void ParseInput(string input){
        // Debug.Log(input);
        string[] splitInput = input.Split(' ');
        velocity.x = float.Parse(splitInput[0]);
        velocity.y = float.Parse(splitInput[1]);
        transform.Rotate(float.Parse(splitInput[3]) * speed *Time.deltaTime, float.Parse(splitInput[2])* speed *Time.deltaTime, 0);
    }
}
