using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTarget : MonoBehaviour {

    public GameObject shootExplosion;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Shoot")
        {
            GameObject explosion = Instantiate(shootExplosion, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
            Destroy(explosion, 1f);
            // Life decrement
            Life targetLife = GetComponent<Life>();
            if(targetLife != null)
            {
                targetLife.Hurt(1);
            }
        }
        
    }
}
