using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowConnectedPlayers : MonoBehaviour
{
    TextMeshProUGUI playerCount;
    public RichTextTagAttribute richTextTagAttribute;
    public string text;

    void Start()
    {
        playerCount = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerCount.text = SimpleServerDemo.instance.clientInfoList.Count + "/" + SimpleServerDemo.instance.MaxPlayerCount + " players connected";
    }
}
