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
    [Header("Fighter Spawn Points")]
    List<Vector3> SpawnPositions = new List<Vector3>();
    [Header("Hit Stop Time")]
    public float hitStopTime = 0.1f;

    #region delegates
    public delegate void FighterHurt(GameObject attacker, GameObject victim, int damage);
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

    void FighterWasHurt(GameObject attacker, GameObject victim, int damage)
    {
        //TODO(PM): remove debug log statement

        if (attacker == victim) return;
        HitStop(attacker, victim, damage);

        Debug.LogWarning("Fighter " + victim + " was hurt for " + damage + " damage!");

        activeFighters[victim].GetHit(attacker.transform.position, damage);
        SimpleServerDemo.SendMessageToClient?.Invoke("vibrate", activeFighters[victim].playerId);
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
        activeFighters[newFighter].selectedCharacterId = fighterType;
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
                activeFighters[fighter].health = 0;
                OnFighterHurt?.Invoke(activeFighters[fighter].gameObject, activeFighters[fighter].gameObject, -activeFighters[fighter].health);
                if (activeFighters[fighter].values.lives <= 0)
                {
                    RemoveFighter(activeFighters[fighter].playerId);
                    break;
                }
            }
        }

        if (activeFighters.Count == 1)
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

    //Functions for hitstop between two fighters
    public void HitStop(GameObject fighter1, GameObject fighter2, int damage)
    {
        //StartCoroutine(HitStopCoroutine(activeFighters[fighter1], activeFighters[fighter2]));
    }
    IEnumerator HitStopCoroutine(FighterController fighter1, FighterController fighter2)
    {
        fighter1.rb.constraints = RigidbodyConstraints.FreezeAll;
        fighter1.animator.enabled = false;
        Vector3 storedVelocity1 = fighter1.GetComponent<Rigidbody>().velocity;
        fighter2.rb.constraints = RigidbodyConstraints.FreezeAll;
        fighter2.animator.enabled = false;
        Vector3 storedVelocity2 = fighter2.GetComponent<Rigidbody>().velocity;
        
        yield return new WaitForSecondsRealtime(hitStopTime * fighter2.health / 100f);
        
        fighter1.animator.enabled = true;
        fighter1.rb.constraints = RigidbodyConstraints.FreezePositionZ;
        fighter1.rb.constraints = RigidbodyConstraints.FreezeRotationX;
        fighter1.rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        fighter1.GetComponent<Rigidbody>().velocity = storedVelocity1;
        
        fighter2.animator.enabled = true;
        fighter2.rb.constraints = RigidbodyConstraints.FreezePositionZ;
        fighter2.rb.constraints = RigidbodyConstraints.FreezeRotationX;
        fighter2.rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        fighter2.GetComponent<Rigidbody>().velocity = storedVelocity2;
    }


}
