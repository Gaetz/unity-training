using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    public int hp;
    public GameObject deathExplosion;
    bool hasDied = false;

    public void Hurt(int lostHp)
    {
        hp -= lostHp;
        if(hp <= 0 && !hasDied)
        {
            Die();
            hasDied = true;
        }
    }

    void Die()
    {
        GameObject explosion = Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(explosion, 1.5f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncreaseScore(1);
    }

}
