using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageUIManager : MonoBehaviour
{
    [SerializeField] FighterManager manager;
    [SerializeField] GameObject PercentageUIui;
    //This script literally only exists to spawn in the UI Elements
    private void Awake()
    {
        foreach (FighterController fighter in manager.activeFighters.Values)
        {
            PercentageUI characterUI = Instantiate(PercentageUIui, transform).GetComponent<PercentageUI>();
            characterUI.SetValues(fighter.playerId);
        }
    }
}
