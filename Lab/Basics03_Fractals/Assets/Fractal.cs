using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

	[SerializeField]
	private Mesh mesh;

	[SerializeField]
	private Material material;

	[SerializeField]
	private int maxDepth;
	private int depth;

	[SerializeField]
	private float childScale;

	[SerializeField]
	private float maxRotationSpeed;
	private float rotationSpeed;

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f)
	};


	void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;
		rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
		if (depth < maxDepth) {
			StartCoroutine(CreateChildren());
		}
	}

	private IEnumerator CreateChildren() {
		for (int i = 0; i < childDirections.Length; i++) {
			yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
			new GameObject("Fractal child").AddComponent<Fractal>().
				Initialize(this, i);
		}
	}

	private void Initialize(Fractal parent, int childIndex) {
		mesh = parent.mesh;
		material = parent.material;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		maxRotationSpeed = parent.maxRotationSpeed;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}
	
	void Update () {
		transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
