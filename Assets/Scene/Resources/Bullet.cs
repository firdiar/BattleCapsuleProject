using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float Damage{ get; set; }
	public float Speed{ get; set; }
	public Vector3 target;
	Rigidbody rb;
	void Awake(){
		rb = GetComponent<Rigidbody> ();
		Destroy (gameObject, 8);
	}
	public void setColor(Color c){
		GetComponent<Renderer> ().material.color = c;
	}

	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, target, Speed * Time.deltaTime);
		transform.LookAt (target);

		if (Vector3.Distance (transform.position, target) < 0.5f) {
			Vector3 moveAdd = target - transform.position;
			target = target + (moveAdd*100);
		}
	}

	void OnCollisionEnter(Collision col){
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "Map") {
			Destroy (this.gameObject);
		}


	}
}
