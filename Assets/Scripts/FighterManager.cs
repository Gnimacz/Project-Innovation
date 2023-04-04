using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FighterManager : MonoBehaviour
{
    public List<GameObject> fighterPrefabs;
    public Dictionary<GameObject, FighterController> activeFighters = new Dictionary<GameObject, FighterController>();

    //delegates
    public delegate void FighterHurt(GameObject fighter, int damage);
    public static FighterHurt OnFighterHurt;

    void FighterWasHurt(GameObject fighter, int damage)
    {
        //TODO(PM): remove debug log statement
        Debug.LogWarning("Fighter " + fighter + " was hurt for " + damage + " damage!");
        
        activeFighters[fighter].GetHit(transform.position, damage);
        // SimpleServerDemo.SendMessageToClient?.Invoke("vibrate", activeFighters[fighter].playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        //testers for keyboard
        SpawnFighter(null, 0);
        SpawnFighter(null, 1);
        
        OnFighterHurt += FighterWasHurt;
        //subscribe to events
        InputEvents.ClientConnected += SpawnFighter;
        InputEvents.ClientDisconnected += RemoveFighter;
    }


    public void SpawnFighter(object sender, int fighterId)
    {
        GameObject newFighter = Instantiate(fighterPrefabs[0], transform.position, quaternion.identity);
        newFighter.transform.parent = transform;
        activeFighters.Add(newFighter, newFighter.GetComponent<FighterController>());
        activeFighters[newFighter].playerId = fighterId;
        // newFighter.GetComponent<FighterController>().playerId = fighterId;
    }
    public void RemoveFighter(object sender, int fighterId)
    {
        Debug.LogWarning("Removing fighter with id: " + fighterId);
        foreach (GameObject fighter in activeFighters.Keys)
        {
            if (activeFighters[fighter].playerId == fighterId)
            {
                activeFighters.Remove(fighter);
                Destroy(fighter);
                break;
            }
        }
        
    }
}
