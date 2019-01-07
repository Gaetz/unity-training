using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour {

  protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	[SerializeField] protected int horizontalRayCount = 4;
	[SerializeField] protected int verticalRayCount = 4;

	protected float horizontalRaySpacing;
	protected float verticalRaySpacing;
	protected const float skinWidth = 0.015f;
	protected BoxCollider2D boxCollider;
	protected RaycastOrigins raycastOrigins;

	protected void UpdateRaycastOrigins() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	protected void CalculateRaySpacing() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	protected struct RaycastOrigins {
		public Vector2 topLeft, topRight, bottomLeft, bottomRight;
	}
}
