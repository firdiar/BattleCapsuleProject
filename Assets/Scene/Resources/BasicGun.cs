using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : MonoBehaviour , IDamageGiver {

	public int Ammo{ get; set; }
	public int MaxInMagazine{ get; set; }
	public int InMagazine { get; set; }
	[SerializeField] float DamageWpnMin = 0;
	[SerializeField] float DamageWpnMax = 0;
	[SerializeField] float speedbullet = 0;

	[SerializeField]GameObject spawnBullet = null;
	[SerializeField]GameObject bullet = null;

	PlayerActiveController PAC;
	bool iniMiliku;
	PhotonView pv;



	// Use this for initialization
	void Start () {
		

		MaxInMagazine = 30;
		InMagazine = MaxInMagazine;


	}
	void Awake(){
		iniMiliku = false;
		StartCoroutine (initialize ());
		BtnShoot ();

		//pv = GetComponent<PhotonView> ();
//		GameObject[] parent = GameObject.FindGameObjectsWithTag ("Player");
//		foreach (GameObject c in parent) {
//			if (c.GetComponent<PhotonView> ().isMine) {
//				transform.SetParent (c.transform.GetChild (0).GetChild (1).transform);
//				break;
//			}
//		}
	}
	IEnumerator initialize(){
		yield return new WaitForSeconds (1);
		if (transform.parent == null) {
			
			// Destroy (this.gameObject);
		} else {
			iniMiliku = transform.parent.parent.parent.GetComponent<PhotonView> ().isMine;
			PAC = transform.parent.parent.parent.GetComponent<PlayerActiveController> ();

			//Debug.Log (iniMiliku);
			if (iniMiliku) {
				GameObject canvas = GameObject.Find ("Canvas");
				UnityEngine.UI.Button btnShoot = canvas.transform.GetChild (1).GetComponent<UnityEngine.UI.Button> ();
				UnityEngine.UI.Button btnReload = canvas.transform.GetChild (2).GetComponent<UnityEngine.UI.Button> ();
				btnShoot.onClick.AddListener (BtnShoot);
				btnReload.onClick.AddListener (BtnReload);
			} 
		}
	}


	// Update is called once per frame
	void Update () {
		if (!iniMiliku)
			return;
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.R)) {
			BtnReload ();
		}
		if (Input.GetMouseButton(0)) {
			BtnReload ();
		}
		#endif
	}

	public void BtnShoot(){
		if (InMagazine <= 0)
			return;
		Debug.Log ("Im clik shoot");
		InMagazine -= 1;
	    float DMG =  Random.Range( DamageWpnMin , DamageWpnMax);
		Vector3 target = PAC.GetPointShoot ();
		Color C = PAC.GetMyColor ();
		GetComponent<PhotonView>().RPC ("Shoot", PhotonTargets.All , spawnBullet.transform.position ,target , DMG , C.r , C.g , C.b , C.a);

	}


	[PunRPC]
	public void Shoot(Vector3 pos  , Vector3 target, float dmg , float R , float G , float B , float A){

		GameObject temp = Instantiate (bullet, pos , Quaternion.identity);
		Bullet b = temp.GetComponent<Bullet> ();
		b.Damage = dmg;
		b.Speed = speedbullet;
		b.target = target;
		b.setColor (new Color(R , G , B , A));
		Debug.Log (new Color(R , G , B , A));

		Debug.Log (InMagazine);
	}

//	public void SetBulletColor(Color c){
//		Debug.Log (c);
//		bulletColor = c;
//	}



	public void BtnReload (){
		StartCoroutine (Reloading ());
	}

	IEnumerator Reloading(){
		yield return new WaitForSeconds (1.5f);
		InMagazine = MaxInMagazine;

	}
}
