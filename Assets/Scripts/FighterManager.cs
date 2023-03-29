using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FighterManager : MonoBehaviour
{
    public List<GameObject> fighterPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newFighter = Instantiate(fighterPrefabs[0], new Vector3(0,1,0), quaternion.identity);
        newFighter.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
