using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private FighterController fighterController;
    private void Start() {
        playerName.text = "P" + ((int)fighterController.playerId + 1);
        playerName.color = PlayerColors.colors[fighterController.playerId];
        indicatorImage.color = PlayerColors.colors[fighterController.playerId];
    }

    private void Update(){
        FaceCamera();
    }
    void FaceCamera(){
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
