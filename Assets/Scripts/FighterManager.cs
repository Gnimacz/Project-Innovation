using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using WebSockets;

public class FighterManager : MonoBehaviour
{
    public List<GameObject> fighterPrefabs;
    public Dictionary<GameObject, FighterController> activeFighters = new Dictionary<GameObject, FighterController>();

    [Header("Screen Bounds")]
    public Vector4 screenBounds = Vector4.zero;

    #region delegates
    public delegate void FighterHurt(Vector3 attackerPosition, GameObject fighter, int damage);
    public static FighterHurt OnFighterHurt;

    public delegate void onFighterSpawned(GameObject fighter, FighterController fighterController);
    public static onFighterSpawned fighterSpawned;

    public delegate void DisableAllFighterInputs();
    public static DisableAllFighterInputs disableAllFighterInputs;
    public delegate void EnableAllFighterInputs();
    public static EnableAllFighterInputs enableAllFighterInputs;
    public delegate void PlayerWon(int id);
    public static PlayerWon OnPlayerWon;
    #endregion

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
        OnFighterHurt += FighterWasHurt;

        disableAllFighterInputs += DisableAllInputs;
        enableAllFighterInputs += EnableAllInputs;
    }

    void Awake()
    {
        if (SimpleServerDemo.instance.clientInfoList.Count < 1)
        {
            SpawnFighter(null, 0);
            SpawnFighter(null, 1);
            Debug.LogWarning("No Connections found, assuming test scenario. Spawning fighters from awake");
            return;
        }
        foreach (Tuple<WebSocketConnection, int, int> connection in SimpleServerDemo.instance.clientInfoList)
        {
            SpawnFighter(connection.Item2, connection.Item3 - 1);
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
    public void RemoveFighter(int fighterId)
    {
        Debug.LogWarning("Removing fighter with id: " + fighterId);
        Dictionary<GameObject, FighterController> newFighterHolder = activeFighters;
        foreach (GameObject fighter in activeFighters.Keys)
        {
            if (activeFighters[fighter].playerId == fighterId)
            {
                newFighterHolder.Remove(fighter);
                Destroy(fighter);
                break;
            }
        }
        activeFighters = newFighterHolder;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(transform.position.x + screenBounds.x, transform.position.y + screenBounds.y, 0), new Vector3(transform.position.x + screenBounds.z, transform.position.y + screenBounds.y, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + screenBounds.x, transform.position.y + screenBounds.w, 0), new Vector3(transform.position.z + screenBounds.z, transform.position.y + screenBounds.w, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + screenBounds.x, transform.position.y + screenBounds.y, 0), new Vector3(transform.position.x + screenBounds.x, transform.position.y + screenBounds.w, 0));
        Gizmos.DrawLine(new Vector3(transform.position.z + screenBounds.z, transform.position.y + screenBounds.y, 0), new Vector3(transform.position.z + screenBounds.z, transform.position.y + screenBounds.w, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + screenBounds.x, transform.position.y + screenBounds.y, 0), new Vector3(transform.position.z + screenBounds.z, transform.position.y + screenBounds.w, 0));
        Gizmos.DrawLine(new Vector3(transform.position.x + screenBounds.x, transform.position.y + screenBounds.w, 0), new Vector3(transform.position.z + screenBounds.z, transform.position.y + screenBounds.y, 0));
    }
    
    //kill the fighter if it's outside the screen bounds
    void Update()
    {
        foreach (GameObject fighter in activeFighters.Keys)
        {

            if (!IsFighterWithinScreenBounds(activeFighters[fighter]))
            {
                activeFighters[fighter].transform.position = transform.position;
                activeFighters[fighter].values.lives--;
                if (activeFighters[fighter].values.lives <= 0)
                {
                    RemoveFighter(activeFighters[fighter].playerId);
                    break;
                }
            }
        }

        if(activeFighters.Count == 1)
        {
            KeyValuePair<GameObject, FighterController> test = activeFighters.First();
            int idOfFighterWon = test.Value.playerId;
            OnPlayerWon?.Invoke(idOfFighterWon);
            disableAllFighterInputs?.Invoke();
            RemoveFighter(idOfFighterWon);
        }
    }
    bool IsFighterWithinScreenBounds(FighterController fighter)
    {
        return ((fighter.gameObject.transform.position.x > (transform.position.x + screenBounds.x)
            && fighter.gameObject.transform.position.x < (transform.position.x + screenBounds.z))
            && (fighter.transform.position.y < transform.position.y + screenBounds.z)
            && (fighter.transform.position.y > transform.position.y + screenBounds.w));
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(fighter.transform.position);
        //return screenPos.x > screenBounds.x && screenPos.x < screenBounds.z && screenPos.y > screenBounds.y && screenPos.y < screenBounds.w;
    }

    public void DisableAllInputs()
    {
        foreach (GameObject fighter in activeFighters.Keys)
        {
            activeFighters[fighter].enabled = false;
        }
    }
    public void EnableAllInputs()
    {
        foreach (GameObject fighter in activeFighters.Keys)
        {
            activeFighters[fighter].enabled = true;
        }
    }
}
