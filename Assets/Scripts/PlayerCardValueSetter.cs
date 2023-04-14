using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCardValueSetter : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    // Start is called before the first frame update
    void Start()
    {
        playerText = GetComponent<TextMeshProUGUI>();
    }
}
