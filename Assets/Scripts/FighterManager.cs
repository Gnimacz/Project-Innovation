using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FighterManager : MonoBehaviour
{
    public List<GameObject> fighterPrefabs;
    public List<GameObject> activeFighters;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newFighter = Instantiate(fighterPrefabs[0], new Vector3(0,1,0), quaternion.identity);
        newFighter.transform.parent = transform;

        //subscribe to events
        InputEvents.ClientConnected += SpawnFighter;
        InputEvents.ClientDisconnected += RemoveFighter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFighter(object sender, int fighterId)
    {
        GameObject newFighter = Instantiate(fighterPrefabs[0], transform.position, quaternion.identity);
        newFighter.transform.parent = transform;
        newFighter.GetComponent<FighterController>().playerId = fighterId;
        activeFighters.Add(newFighter);
    }
    public void RemoveFighter(object sender, int fighterId)
    {
        foreach (GameObject fighter in activeFighters)
        {
            if (fighter.GetComponent<FighterController>().playerId == fighterId)
            {
                activeFighters.Remove(fighter);
                Destroy(fighter);
                break;
            }
        }
        
    }
}
