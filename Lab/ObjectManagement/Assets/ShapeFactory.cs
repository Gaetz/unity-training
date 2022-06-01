using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject {

	[SerializeField] Shape[] prefabs;
	[SerializeField] Material[] materials;
	[SerializeField] bool recycle;

	List<Shape>[] pools;
	
	// Scene that will contain instanced shapes
	private Scene poolScene;

	public Shape Get (int shapeId = 0, int materialId = 0) {
		Shape instance;
		if (recycle) {
			if (pools == null) {
				CreatePools();
			}
			List<Shape> pool = pools[shapeId];
			int lastIndex = pool.Count - 1;
			if (lastIndex >= 0) {
				instance = pool[lastIndex];
				instance.gameObject.SetActive(true);
				pool.RemoveAt(lastIndex);
			} else {
				instance = Instantiate(prefabs[shapeId]);
				instance.ShapeId = shapeId;
				SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);
			}
		} else {
			instance = Instantiate(prefabs[shapeId]);
			instance.ShapeId = shapeId;
		}
		instance.SetMaterial(materials[materialId], materialId);
		return instance;
	}

	public Shape GetRandom () {
		return Get(Random.Range(0, prefabs.Length), Random.Range(0, materials.Length));
	}

	void CreatePools () {
		pools = new List<Shape>[prefabs.Length];
		for (int i = 0; i < pools.Length; i++) {
			pools[i] = new List<Shape>();
		}

		// Allow recompilation for scene pooling
		if (Application.isEditor)
		{
			
			poolScene = SceneManager.GetSceneByName(name);
			if (poolScene.isLoaded)
			{
				// Repopulate the shape list with already used shapes 
				GameObject[] rootObjects = poolScene.GetRootGameObjects();
				for (int i = 0; i < rootObjects.Length; i++)
				{
					Shape pooledShape = rootObjects[i].GetComponent<Shape>();
					
					// If the shape is not active, it should be added to the awaiting pool
					if (pooledShape.gameObject.activeSelf)
					{
						pools[pooledShape.ShapeId].Add(pooledShape);
					}
				}
				return;
			}
		}

		// Shapes created in a specific scene
		poolScene = SceneManager.CreateScene(name);
	}

	public void Reclaim (Shape shapeToRecycle) {
		if (recycle) {
			if (pools == null) {
				CreatePools();
			}
			pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
			shapeToRecycle.gameObject.SetActive(false);
		} else {
			Destroy(shapeToRecycle.gameObject);
		}
	}
}
