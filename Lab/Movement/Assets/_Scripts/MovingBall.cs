using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBall : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] float maxSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput = Vector2.zero;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        Vector2.ClampMagnitude(playerInput, 1.0f);
        Vector3 velocity = new Vector3(playerInput.x, 0, playerInput.y) * maxSpeed;
        Vector3 move = velocity * Time.deltaTime;
        transform.localPosition += move;
    }
}
