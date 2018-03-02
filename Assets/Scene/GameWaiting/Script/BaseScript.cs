using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour {
	[SerializeField]GameManagerScript GMS = null;

	int limitPlayer;
	int player;
	// Use this for initialization
	void Start () {
		player = 0;
		limitPlayer = Mathf.CeilToInt( GMS.playerInRoom/2.0f );
	}
	
	// Update is called once per frame
	void Update () {
		if (player >= limitPlayer) {
			ActiveCollider ();
		}
		
		
	}
	void OnTriggerEnter(Collider col){
		if (col.transform.tag == "Player") {
			player += 1;
		}
	
	}
	void OnTriggerExit(Collider col){
		if (col.transform.tag == "Player") {
			player -= 1;
		}

	}
	public void DeactiveCollider(){
		transform.GetChild (0).GetComponent<Collider> ().isTrigger = true;
	}
	public void ActiveCollider(){
		

		transform.GetChild (0).GetComponent<Collider> ().isTrigger = false;
	}
}
