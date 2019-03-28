using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectInfo : MonoBehaviour {

  public bool IsSelected;
  
  [SerializeField] string objectName;

  NavMeshAgent agent;

	void Start () {
		IsSelected = false;
    agent = GetComponent<NavMeshAgent>();
	}

  void Update() {
    if(Input.GetMouseButtonDown(1) && IsSelected) {
      MoveCommand();
    }
  }

  void MoveCommand() {
    
  }
}
