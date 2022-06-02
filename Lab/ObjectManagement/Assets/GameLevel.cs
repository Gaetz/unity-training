using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private SpawnZone spawnZone;
    
    void Start()
    {
        Game.Instance.SpawnZoneOfLevel = spawnZone;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
