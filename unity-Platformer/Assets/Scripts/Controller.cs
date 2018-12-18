using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
public class Controller : MonoBehaviour {

	[SerializeField] LayerMask collisionMask;
	[SerializeField] int horizontalRayCount = 4;
	[SerializeField] int verticalRayCount = 4;
	
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
		collisions.Reset();
		UpdateRaycastOrigins();
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
				velocity.x = (hit.distance - skinWidth) * rayDirectionX;
				rayMagnitude = hit.distance; // Limit future rays magnitudes
				collisions.left = (rayDirectionX == -1);
				collisions.right = (rayDirectionX == 1);
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity) {
		float rayDirectionY = Mathf.Sign(velocity.y);
		float rayMagnitude = Mathf.Abs(velocity.y) + skinWidth;

		for(int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (rayDirectionY == -1)? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * rayDirectionY, rayMagnitude, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * rayDirectionY * rayMagnitude, Color.red);
			if (hit) {
				velocity.y = (hit.distance - skinWidth) * rayDirectionY;
				rayMagnitude = hit.distance; // Limit future rays magnitudes
				collisions.below = (rayDirectionY == -1);
				collisions.above = (rayDirectionY == 1);
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
		public bool above, below, left, right;

		public void Reset() {
			above = below = left = right = false;
		}
	}
}
