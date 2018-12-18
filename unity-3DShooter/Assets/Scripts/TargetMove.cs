using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    Left, Down, Right, Up
}

public class TargetMove : MonoBehaviour {

    public float speed;

    Rigidbody rbody;

	void Awake () {
        int direction = Random.Range(0, 4);
        rbody = GetComponent<Rigidbody>();
        switch (direction)
        {
            case (int)Direction.Left:
                transform.position = new Vector3(1000, transform.lossyScale.y / 2, Random.Range(-1000f, 1000f));
                rbody.velocity = new Vector3(-speed, 0, 0);
                break;
            case (int)Direction.Down:
                transform.position = new Vector3(Random.Range(-1000f, 1000f), transform.lossyScale.y / 2, -1000);
                rbody.velocity = new Vector3(0, 0, speed);
                break;
            case (int)Direction.Right:
                transform.position = new Vector3(-1000, transform.lossyScale.y / 2, Random.Range(-1000f, 1000f));
                rbody.velocity = new Vector3(speed, 0, 0);
                break;
            case (int)Direction.Up:
                transform.position = new Vector3(Random.Range(-1000f, 1000f), transform.lossyScale.y / 2, 1000);
                rbody.velocity = new Vector3(0, 0, -speed);
                break;
        }

    }
	
}
