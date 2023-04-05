using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private FighterController fighterController;
    private void Start() {
        playerName.text = "P" + ((int)fighterController.PlayerId + 1);
    }

    private void Update(){
        FaceCamera();
    }
    void FaceCamera(){
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
