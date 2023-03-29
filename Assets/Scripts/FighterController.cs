using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public int playerId = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        InputEvents.JumpButtonPressed += OnJumpButtonPressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnJumpButtonPressed(object sender, int id)
    {
        if (id == playerId)
        {
            transform.position += Vector3.up * 5;
        }
    }
    
}
