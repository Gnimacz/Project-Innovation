using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using WebSockets;

public class FighterManager : MonoBehaviour
{
    public List<GameObject> fighterPrefabs;
    public Dictionary<GameObject, FighterController> activeFighters = new Dictionary<GameObject, FighterController>();

    //delegates
    public delegate void FighterHurt(Vector3 attackerPosition, GameObject fighter, int damage);
    public static FighterHurt OnFighterHurt;

    public delegate void onFighterSpawned(GameObject fighter, FighterController fighterController);
    public static onFighterSpawned fighterSpawned;

    void FighterWasHurt(Vector3 attackerPosition, GameObject fighter, int damage)
    {
        //TODO(PM): remove debug log statement
        Debug.LogWarning("Fighter " + fighter + " was hurt for " + damage + " damage!");
        
        activeFighters[fighter].GetHit(attackerPosition, damage);
        SimpleServerDemo.SendMessageToClient?.Invoke("vibrate", activeFighters[fighter].playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        //testers for keyboard
        SpawnFighter(null, 0);
        SpawnFighter(null, 1);
        
        OnFighterHurt += FighterWasHurt;
    }

    void Awake()
    {
        if(SimpleServerDemo.instance.clientInfoList.Count < 1){
            SpawnFighter(null, 0);
            SpawnFighter(null, 1);
            Debug.LogWarning("No Connections found, assuming test scenario. Spawning fighters from awake");
            return;
        }
        foreach(Tuple<WebSocketConnection, int, int> connection in SimpleServerDemo.instance.clientInfoList)
        {
            SpawnFighter(connection.Item2, connection.Item3);
        }
    }

    public void SpawnFighter(int fighterId, int fighterType)
    {
        GameObject newFighter = Instantiate(fighterPrefabs[fighterType], transform.position, quaternion.identity);
        newFighter.transform.parent = transform;
        activeFighters.Add(newFighter, newFighter.GetComponent<FighterController>());
        activeFighters[newFighter].playerId = fighterId;
        // newFighter.GetComponent<FighterController>().playerId = fighterId;
        fighterSpawned?.Invoke(newFighter, activeFighters[newFighter]);
    }
    public void SpawnFighter(object sender, int fighterId)
    {
        GameObject newFighter = Instantiate(fighterPrefabs[0], transform.position, quaternion.identity);
        newFighter.transform.parent = transform;
        activeFighters.Add(newFighter, newFighter.GetComponent<FighterController>());
        activeFighters[newFighter].playerId = fighterId;
        // newFighter.GetComponent<FighterController>().playerId = fighterId;
        fighterSpawned?.Invoke(newFighter, activeFighters[newFighter]);
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
