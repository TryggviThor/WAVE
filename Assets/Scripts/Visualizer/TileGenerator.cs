using UnityEngine;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour {

	public GameObject prefab;
	public int width = 20;
	public int length = 20;

	List<Vector2> generationList = new List<Vector2> ();
	int distIndex;
	BoxCollider coll;

	Vector3 pos;
	GameObject spawnedTile;

	void Awake () {
		coll = GetComponent<BoxCollider>();
		coll.size = new Vector3 (width, 1, length);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < length; y++) {
				generationList.Add (new Vector2 (x + 0.5f, y + 0.5f));
			}
		}

		Generate ();
	}

	
	int Generate () {
		// Generate all tiles in a sequence
		for (int i = 0; i < generationList.Count; i++) {
			GenerateTile (generationList[i]);
		}

		return 0;
	}

	void GenerateTile (Vector2 vc2) {
		// change the vc2 to vc3 and move it to the center
		
		pos = new Vector3(vc2.x - width / 2,
						  0.0f,
						  vc2.y - length / 2);

		pos = transform.TransformPoint (pos);

		spawnedTile = Instantiate (prefab, pos, transform.rotation) as GameObject;
		spawnedTile.transform.localScale = transform.localScale;
		spawnedTile.name = "Tile";
		spawnedTile.transform.parent = transform;
	}

	void OnDrawGizmos () {
		if (Application.isPlaying) return;

		Gizmos.color = Color.white;
		Matrix4x4 rotationMatrix = Matrix4x4.TRS (new Vector3 (transform.position.x, transform.position.y, transform.position.z) + transform.up * 0.5f, 
												  transform.rotation,
												  new Vector3 (width * transform.localScale.x, transform.localScale.y, length * transform.localScale.z));
		Gizmos.matrix = rotationMatrix;
		Gizmos.DrawCube (Vector3.zero, Vector3.one);
	}
}
