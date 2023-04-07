using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 10;
    
    private void OnTriggerEnter(Collider other)
    {
        FighterManager.OnFighterHurt?.Invoke(other.gameObject, damage);
    }
}
