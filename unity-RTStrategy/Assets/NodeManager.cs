using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

	public enum ResourceType {
		Stone
	}
	
	public int Gatherers;
	public ResourceType NodeResourceType;

	[SerializeField] float harvestTime;
	[SerializeField] float availableResource;

	// Use this for initialization
	void Start () {
		StartCoroutine(ResourceTick());
	}
	
	// Update is called once per frame
	void Update () {
		if (availableResource <= 0) {
			Destroy(gameObject);
		}
	}

	IEnumerator ResourceTick() {
		while(true) {
			yield return new WaitForSeconds(1);
			ResourceGather();
		}
	}

	public void ResourceGather() {
		if(Gatherers > 0) {
			availableResource -= Gatherers;
		}
	}
}
