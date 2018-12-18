using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
public class Controller : MonoBehaviour {

	BoxCollider2D collider;

	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
