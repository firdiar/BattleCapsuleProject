using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpnParentController : MonoBehaviour {



	void Awake(){
		GameObject[] player = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject c in player) {
			if (c.GetComponent<PhotonView> ().owner == GetComponent<PhotonView>().owner) {
				ChangeParent (c);
				break;
			}
		
		}



	}


	void ChangeParent(GameObject obj){
		obj.transform.SetParent (obj.transform.GetChild (0).GetChild (1).transform);
		transform.GetChild (0).GetChild (1).transform.rotation = Quaternion.Euler (0, 270, 0);
	}
}
