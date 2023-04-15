using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DamageDealer : MonoBehaviour
{
    public int damage = 10;
    [SerializeField] private VisualEffect VFXToPlay;
    
    private void OnTriggerEnter(Collider other)
    {
        FighterManager.OnFighterHurt?.Invoke(transform.parent.parent.gameObject, other.gameObject.transform.parent.gameObject, damage);
        // FighterManager.OnFighterHurt?.Invoke(transform.position, other.gameObject, damage);
        if (VFXToPlay == null) return;
        VFXToPlay.Play();
    }
}
