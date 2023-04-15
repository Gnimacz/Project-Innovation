using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowConnectedPlayers : MonoBehaviour
{
    TextMeshProUGUI playerCount;
    void Start()
    {
        playerCount = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerCount.text = "Connected Players: " + SimpleServerDemo.instance.clientInfoList.Count + "/" + SimpleServerDemo.instance.MaxPlayerCount;
    }
}
