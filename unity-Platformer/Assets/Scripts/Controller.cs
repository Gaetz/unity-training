using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
public class Controller : MonoBehaviour {

	[SerializeField] LayerMask collisionMask;
	[SerializeField] int horizontalRayCount = 4;
	[SerializeField] int verticalRayCount = 4;
	[SerializeField] int maxClimbAngle = 80;
	[SerializeField] int maxDescendAngle = 75;

	public CollisionInfo Collisions {
		get {
			return collisions;
		}
	}
	CollisionInfo collisions;

	float horizontalRaySpacing;
	float verticalRaySpacing;
	const float skinWidth = 0.015f;
	BoxCollider2D boxCollider;
	RaycastOrigins raycastOrigins;

    void Start () {
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

    public void Move(Vector3 velocity)
    {
		UpdateRaycastOrigins();
		collisions.Reset();
		collisions.velocityOld = velocity;

		if(velocity.y < 0) {
			DescendSlope(ref velocity);
		}
		if(velocity.x != 0) {
			HorizontalCollisions(ref velocity);
		}
		if(velocity.y != 0) {
			VerticalCollisions(ref velocity);
		}
		transform.Translate(velocity);
    }

	void HorizontalCollisions(ref Vector3 velocity) {
		float rayDirectionX = Mathf.Sign(velocity.x);
		float rayMagnitude = Mathf.Abs(velocity.x) + skinWidth;

		for(int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (rayDirectionX == -1)? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * rayDirectionX, rayMagnitude, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * rayDirectionX * rayMagnitude, Color.red);
			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal, Vector3.up);
				if(slopeAngle <= maxClimbAngle && i == 0) {

					if(collisions.descendingSlope) {
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}

					float distanceToSlopeStartX = 0;
					if(slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStartX = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStartX * rayDirectionX;
					}

					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStartX * rayDirectionX;
				}

				/* if(collisions.climbingSlope) {
					velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
				}*/

				if(!collisions.climbingSlope || collisions.slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * rayDirectionX;
					rayMagnitude = hit.distance; // Limit future rays magnitudes

					collisions.left = (rayDirectionX == -1);
					collisions.right = (rayDirectionX == 1);
				}
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity) {
		float rayDirectionY = Mathf.Sign(velocity.y);
		float rayMagnitude = Mathf.Abs(velocity.y) + skinWidth;

		for(int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (rayDirectionY == -1)? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * rayDirectionY, rayMagnitude, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * rayDirectionY * rayMagnitude, Color.red);
			if (hit) {
				velocity.y = (hit.distance - skinWidth) * rayDirectionY;
				rayMagnitude = hit.distance; // Limit future rays magnitudes

				if(collisions.climbingSlope) {
					velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisions.below = (rayDirectionY == -1);
				collisions.above = (rayDirectionY == 1);
			}
		}

		// Avoid one frame stop when changing slope
		if (collisions.climbingSlope) {
			float rayDirectionX = Mathf.Sign(velocity.x);
			rayMagnitude = Mathf.Abs(velocity.x) + skinWidth;
			Vector2 rayOrigin = (rayDirectionX == -1)? raycastOrigins.bottomLeft : raycastOrigins.bottomRight + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * rayDirectionX, rayMagnitude, collisionMask);
			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal, Vector3.up);
				if (slopeAngle != collisions.slopeAngleOld) {
					velocity.x = (hit.distance - skinWidth) * rayDirectionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs(velocity.x);
		float climbVelocityY = Mathf.Sin(Mathf.Deg2Rad * slopeAngle) * moveDistance;
		if(velocity.y <= climbVelocityY) {
			velocity.x = Mathf.Cos(Mathf.Deg2Rad * slopeAngle) * moveDistance * Mathf.Sign(velocity.x);
			velocity.y = climbVelocityY;
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity) {
		float rayDirectionX = Mathf.Sign(velocity.x);
		Vector2 rayOrigin = (rayDirectionX == -1)? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector3.up);
			if(slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				// Moving down the slop
				if (Mathf.Sign(hit.normal.x) == rayDirectionX) {
					float yMove = Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					if(hit.distance - skinWidth <= yMove) {
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin(Mathf.Deg2Rad * slopeAngle) * moveDistance;
						velocity.x = Mathf.Cos(Mathf.Deg2Rad * slopeAngle) * moveDistance * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;
						collisions.below = true;
						collisions.descendingSlope = true;
						collisions.slopeAngle = slopeAngle;
					}
				}
			}
		}
	}

	void UpdateRaycastOrigins() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight, bottomLeft, bottomRight;
	}

	public struct CollisionInfo {
		public bool above, below, left, right, climbingSlope, descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;

		public void Reset() {
			above = below = left = right = climbingSlope = descendingSlope = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}
}
