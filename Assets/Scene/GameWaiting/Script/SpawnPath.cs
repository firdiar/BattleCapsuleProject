using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPath : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Gizmos.DrawCube (transform.position, new Vector3 (500, 10, 500));
	}
	
	// Update is called once per frame
	void Update () {
		Physics.BoxCast (transform.position, new Vector3 (500, 10, 500), Vector3.forward);

		
	}
}
