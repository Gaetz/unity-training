using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NucleonSpawner : MonoBehaviour {

	[SerializeField] float timeBetweenSpawns;
	[SerializeField] float spawnDistance;
	[SerializeField] Nucleon[] nucleonPrefabs;
	float timeSinceLastSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timeSinceLastSpawn += Time.deltaTime;
		if(timeSinceLastSpawn > timeBetweenSpawns) {
			timeSinceLastSpawn -= timeBetweenSpawns;
			SpawnNucleon();
		}
	}

    private void SpawnNucleon()
    {
        Nucleon prefab = nucleonPrefabs[UnityEngine.Random.Range(0, nucleonPrefabs.Length)];
		Nucleon spawn = Instantiate<Nucleon>(prefab);
		spawn.transform.localPosition = UnityEngine.Random.onUnitSphere * spawnDistance;
    }
}
