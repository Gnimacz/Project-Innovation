using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFramer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private FighterManager fighterManager;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    
    private void Start() {
        FighterManager.fighterSpawned += AddTarget;
    }

    private void AddTarget(GameObject fighter, FighterController fighterController){
        targetGroup.AddMember(fighter.transform, 1, 10);
    }
}