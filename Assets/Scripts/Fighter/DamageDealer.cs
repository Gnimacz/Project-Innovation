using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 10;
    
    private void OnTriggerEnter(Collider other)
    {
        FighterManager.OnFighterHurt?.Invoke(transform.parent.parent.gameObject, other.gameObject.transform.parent.gameObject, damage);
    }
}
