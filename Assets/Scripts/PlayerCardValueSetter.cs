using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCardValueSetter : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    public GameObject yuriImage;
    public GameObject rubenImage;
    public GameObject tacoImage;
    // Start is called before the first frame update
    void Start()
    {
        playerText = GetComponent<TextMeshProUGUI>();
    }
}
