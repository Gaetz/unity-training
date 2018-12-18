using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Controller))]
public class Player : MonoBehaviour {


	[SerializeField] float jumpHeight = 4;
	[SerializeField] float jumpTimeToApex = 0.4f;
	[SerializeField] float accelerationTimeAirborn = 0.2f;
	[SerializeField] float accelerationTimeGrounded = 0.1f;
	[SerializeField] float moveSpeed = 6f;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmooth;
	Controller controller;
	
	void Start () {
		controller = GetComponent<Controller>();
		gravity = -2 * jumpHeight / (jumpTimeToApex * jumpTimeToApex);
		jumpVelocity = Mathf.Abs(gravity) * jumpTimeToApex;
	}
	
	void Update () {
		if(controller.Collisions.above || controller.Collisions.below) {
			velocity.y = 0;
		}

		// Jump
		if (Input.GetKeyDown(KeyCode.Space) && controller.Collisions.below) {
			velocity.y += jumpVelocity;
		}
		// Move
		Vector2 input = new Vector2( Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		float targetVelocityX = input.x * moveSpeed;
		float accelerationTime = (controller.Collisions.below? accelerationTimeGrounded : accelerationTimeAirborn);
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmooth, accelerationTime);
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
