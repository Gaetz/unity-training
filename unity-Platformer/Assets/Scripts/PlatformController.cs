using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {

	[SerializeField] LayerMask passengerMask;
	[SerializeField] Vector2 move;

  protected override void Start () {
		base.Start();
	}
	
	void Update () {
		UpdateRaycastOrigins();
		Vector2 velocity = move * Time.deltaTime;
		MovePassenger(velocity);
		transform.Translate(velocity);
	}

	void MovePassenger(Vector2 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform>();

		float directionX = Mathf.Sign(velocity.x);
		float directionY = Mathf.Sign(velocity.y);

		if(directionY != 0) {
			float rayMagnitude = Mathf.Abs(velocity.y) + skinWidth;

			for(int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = (directionY == -1)? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayMagnitude, passengerMask);

				if(hit) {
					if(!movedPassengers.Contains(hit.transform)) {
						float pushX = (directionY == 1 ? velocity.x : 0); // When we are on the platform and it moves horizontally AND upward
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
						hit.transform.Translate(new Vector2(pushX, pushY));
						movedPassengers.Add(hit.transform);
					}
				}
			}
		}

		if(directionX != 0) {
			float rayMagnitude = Mathf.Abs(velocity.x) + skinWidth;

			for(int i = 0; i < horizontalRayCount; i++) {
				Vector2 rayOrigin = (directionX == -1)? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayMagnitude, passengerMask);
				if(hit) {
					if(!movedPassengers.Contains(hit.transform)) {
						float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
						float pushY = 0;
						hit.transform.Translate(new Vector2(pushX, pushY));
						movedPassengers.Add(hit.transform);
					}
				}
			}
		}

		if(directionY == -1 || (velocity.x != 0 && velocity.y == 0)) {
			float rayMagnitude = skinWidth * 2;

			for(int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayMagnitude, passengerMask);

				if(hit) {
					if(!movedPassengers.Contains(hit.transform)) {
						float pushX = velocity.x;
						float pushY = velocity.y;
						hit.transform.Translate(new Vector2(pushX, pushY));
						movedPassengers.Add(hit.transform);
					}
				}
			}
		}
	}
}
