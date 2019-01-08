using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {

	[SerializeField] LayerMask passengerMask;
	[SerializeField] Vector2 move;


	List<PassengerMovement> passengerMovements;
	Dictionary<Transform, Controller> passengers = new Dictionary<Transform, Controller>();

  	protected override void Start () {
		base.Start();
		passengerMovements = new List<PassengerMovement>();
	}
	
	void Update () {
		UpdateRaycastOrigins();
		Vector2 velocity = move * Time.deltaTime;
		ComputePassengerMove(velocity);
		
		MovePassengers(true);
		transform.Translate(velocity);
		MovePassengers(false);
	}

	void MovePassengers(bool beforeMove) {
		foreach (var move in passengerMovements)
		{
			if(!passengers.ContainsKey(move.transform)) {
				passengers.Add(move.transform, move.transform.GetComponent<Controller>());
			}
			if(move.moveBeforePlatform == beforeMove) {
				passengers[move.transform].Move(move.velocity, move.standingOnPlatform);
			}
		}
	}

	void ComputePassengerMove(Vector2 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform>();
		passengerMovements.Clear();

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
						passengerMovements.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), directionY == 1, true));
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
						float pushY = -skinWidth;
						passengerMovements.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), false, true));
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
						passengerMovements.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), true, false));
						movedPassengers.Add(hit.transform);
					}
				}
			}
		}
	}

	struct PassengerMovement {
		public Transform transform;
		public Vector2 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform transform, Vector2 velocity, bool standingOnPlatform, bool moveBeforePlatform) {
			this.transform = transform;
			this.velocity = velocity;
			this.standingOnPlatform = standingOnPlatform;
			this.moveBeforePlatform = moveBeforePlatform;
		}
	}
}
