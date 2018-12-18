using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    public float speed;
    public float speedVariation;
    public float rollSpeed;
    public float pitchSpeed;
    public float yawSpeed;

    Rigidbody rbody;
    
	void Start () {
        rbody = GetComponent<Rigidbody>();
    }
	
	void Update () {
        // Input
		if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up * rollSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * -rollSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.forward * -pitchSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(Vector3.forward * pitchSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.right * -yawSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.right * yawSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speed += speedVariation;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            speed -= speedVariation;
        }
        // Direction
        rbody.velocity = speed * (transform.rotation * Vector3.up) * Time.deltaTime;
    }
}
