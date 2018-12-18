using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour {

    public GameObject shoot;
    public float cooldown = 0.5f;

    float counter = 1f;
    bool spawnLeftShot = false;
	
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            if(counter > cooldown)
            {
                if(spawnLeftShot)
                {
                    Instantiate(shoot, transform.GetChild(1).position, transform.rotation);
                    spawnLeftShot = false;
                }
                else
                {
                    Instantiate(shoot, transform.GetChild(0).position, transform.rotation);
                    spawnLeftShot = true;
                }
                counter = 0f;
            }
        }
        counter += Time.deltaTime;
    }
}
