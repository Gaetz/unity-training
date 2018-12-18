using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject target;
    public Text scoreText;
    int score;

	void Start () {
        StartCoroutine(SpawnTarget());
        score = 0;
	}
	
	IEnumerator SpawnTarget()
    {
        while(true)
        {
            Instantiate(target);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void IncreaseScore(int plus)
    {
        score += plus;
        scoreText.text = score.ToString();
    }
}
