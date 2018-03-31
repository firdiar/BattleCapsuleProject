using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGatcha : MonoBehaviour {

	[System.Serializable]
	public class Hadiah {

		public int Min;
		public int Max;
		public int animVal;
	}
		
	public ParticleSystem[] circle = new ParticleSystem[3];
	public GameObject[] tube = new GameObject[2];

	[SerializeField]Hadiah[] hadiah = null;
	[SerializeField]GameObject spawnItemPos = null;
	int value;
	int animVal;
	bool clear = false;
	int speedup = 0;
	int maxRate = 0;


	// Use this for initialization
	void Start () {
		maxRate = 60;
		speedup = 5;
		InvokeRepeating ("UpRate", 1, 0.2f);
		transform.eulerAngles = new Vector3 (100, 0, 0);

		value = Random.Range (0, 1001);

		foreach (Hadiah c in hadiah) {
			if ((c.Min < value) && (c.Max > value)) {
				animVal = c.animVal;
				Debug.Log (animVal);
				Debug.Log (value);
			}
		}
	}
	
	// Update is called once per frame
	void UpRate () {
		Debug.Log ("Working");
	
		foreach (ParticleSystem c in circle) {
			var em = c.emission;

			em.rateOverTime =  Mathf.Clamp (em.rateOverTime.constant + speedup, 15,  maxRate);
		}

		if (circle[0].emission.rateOverTime.constant == maxRate) {

			CancelInvoke ("UpRate");
			if (maxRate == 60) {
				clear = true;
				tube [1].transform.GetChild (0).GetChild (0).gameObject.SetActive (true);
			}
		
		}
	}

	void Update(){
		transform.eulerAngles += new Vector3( 0 , 0 , 75);
		spawnItemPos.transform.eulerAngles += new Vector3 (0, 5, 0);
		if (clear) {
			if (maxRate == 60) {
				StartCoroutine (DeactiveParticle ());
				clear = false;
			} else {
				for (int i = 0; i < 5; i++) {
					if(i <=2)
						circle [i].gameObject.transform.localScale = new Vector3 (Mathf.Clamp (circle [i].gameObject.transform.localScale.x + 0.1f , 0 , 3.35f) ,Mathf.Clamp (circle [i].gameObject.transform.localScale.y + 0.1f , 0 , 3.35f) );

					if (i == 0) {
						circle [i].gameObject.transform.localPosition = new Vector3 (0, 0, Mathf.Clamp (circle [i].gameObject.transform.localPosition.z + 0.5f, 0, 3.8f));
					} else if (i == 1) {
						circle [i].gameObject.transform.localPosition = new Vector3 (0, 0, Mathf.Clamp (circle [i].gameObject.transform.localPosition.z - 0.5f, -2.4f, 0));
					} else if (i == 2) {
						circle [i].gameObject.transform.localPosition = new Vector3 (0, 0, Mathf.Clamp (circle [i].gameObject.transform.localPosition.z - 0.5f, -9f, 0));
					}
					var e = circle [i].colorOverLifetime;
					e.enabled = false;
				}
				tube[0].transform.localPosition = new Vector3( 0 , 0 , Mathf.Lerp(tube[0].transform.localPosition.z , -6.25f , 1f));
				tube[1].transform.localPosition = new Vector3( 0 , 0 , Mathf.Lerp(tube[1].transform.localPosition.z , 0.5f , 1f));
				if(circle [2].gameObject.transform.localPosition.z == -9){
					spawnItemPos.SetActive (true);
					GameObject temp = Instantiate (Resources.Load<GameObject> ("BasicGun"), spawnItemPos.transform.position, Quaternion.identity , spawnItemPos.transform);
					temp.transform.localScale *= 2;
					clear = false;
				}
			}
			Debug.Log ("Clear");

		}
	}

	IEnumerator DeactiveParticle(){
		//circle [3].gameObject.SetActive (true);
		//circle [4].gameObject.SetActive (true);
		if (animVal < 3) {
			var em = circle [2].emission;
			em.rateOverTime = 0f;
		}
		if (animVal < 2) {
			var em = circle [1].emission;
			em.rateOverTime = 0f;
		}
		for (int i = 0; i < 3; i++) {
			var e = circle[i].main;
			var em = circle[i].emission;
			e.maxParticles = 800;
			em.rateOverTime = 600;


		}


		yield return new WaitForSeconds (0.7f);

		if (animVal < 3) {
			circle [1].gameObject.SetActive (false);
		}
	
		maxRate = 500;
//		speedup = 100;
//		Debug.Log ("donw");
//		

		clear = true;
	
	}
}
