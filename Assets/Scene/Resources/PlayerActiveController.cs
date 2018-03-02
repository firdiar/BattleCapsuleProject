using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActiveController : MonoBehaviour , IStatus {



	//milik IStatus----------------------------------------
	float _health;
	public float maksHealth;
	RectTransform HPBar;
	private float Health {
		get{
			return _health;
		}
		set{
			float lerp = value / maksHealth;
			HPBar.localPosition = new Vector3 (-400 * (1-lerp), 0 , 0);

		    _health = value;
		}
	}
	private float Armor{
		get;
		set;
	}

	public int LoadCapacity{
		get;
		set;
	}

	public int MedKit{ get; set; }


	public void GetDamage(float Damage){
		Health = Mathf.Clamp( Health - (Damage - Armor) , 0 , Health);
	
	}
	public void GetHeal (float Heal){
		LoadCapacity -= 20;
	}





	public void GetItem (GameObject item){
		switch (item.tag) {

		case "MedKit":
			LoadCapacity += 20;
			MedKit += 1;
			break;
		
		}
	}
	public void AddCapacity (int weightItem){
	
	}
	public void minCapacity (int weightItem){
	
	}
//	public delegate void SetBulletColor(Color c);
//	public event SetBulletColor setBulletColor;

	//private Color myColor;
	private Color MyColor {
		get{ 
			return transform.GetChild(1).GetComponent<Renderer> ().material.color;
		}
		set{


			transform.GetChild(1).GetComponent<Renderer> ().material.color = value;
		}
	} 
	public Color GetMyColor(){
		return MyColor;
	}

	//Selesai milik IStatus------------------------------------

	Transform playerNameTag;
	GameObject mapCameraRenderer;
	GameObject wpn;
	public GameObject nodePlayerBlue;
	public GameObject nodePlayerRed;
	public GameObject nodeGreen;
	GameObject myNode = null;
	public GameObject cam;

	public GameObject spawnPos {
		get;
		set;
	}


	// Use this for initialization
	void Awake() {
		mapCameraRenderer = GameObject.Find ("CameraMapRenderer");
		MyColor = transform.GetChild(1).GetComponent<Renderer> ().material.color;
		HPBar = GameObject.Find ("Canvas").transform.GetChild (5).GetChild (0).GetComponent<RectTransform> ();
		Health = maksHealth;
		Debug.Log (Health);
		Armor = 0;
		LoadCapacity = 200;
		MedKit = 0;
		MyColor = new Color (1, 1, 1 , 1);
		if (!GetComponent<PhotonView> ().isMine) {
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = false;
			GameObject child = transform.GetChild (0).gameObject;
			child.GetComponent<Camera> ().enabled = false;
			child.GetComponent<AudioListener> ().enabled = false;
			child = transform.GetChild (2).gameObject;
			playerNameTag = child.transform;
		} else {
			Input.compass.enabled = true;
			GameObject child = transform.GetChild (2).gameObject;
			playerNameTag = null;
			child.gameObject.SetActive (false);
			//SummonWPN ();
			string wpnName = "BasicGun";

			GetComponent<PhotonView> ().RPC ("SummonWPN", PhotonTargets.All , wpnName);

		}
		GetComponent<PhotonView> ().RPC ("AddNameTag", PhotonTargets.Others, PhotonNetwork.playerName);
		
	}

	[PunRPC]
	void AddNameTag(string name){
		if (name == "")
			name = "Player" + Random.Range (1, 999).ToString ();
		
		playerNameTag.GetComponent<TextMesh> ().text = name;
	}

	[PunRPC]
	void SummonWPN(string nameWeapon){
		GameObject obj = Resources.Load<GameObject> (nameWeapon);
		Instantiate (obj, transform.GetChild (0).GetChild (1).transform.position , Quaternion.Euler(0 , 270 , 0) , this.gameObject.transform.GetChild (0).GetChild (1).transform);
		//wpn = (GameObject)PhotonNetwork.Instantiate ("BasicGun", transform.GetChild (0).GetChild (1).transform.position, Quaternion.identity , 0);
		//wpn.transform.SetParent (this.gameObject.transform.GetChild (0).GetChild (1).transform);
		//wpn.GetComponent<WpnParentController> ().Parent = this.gameObject;
		//Debug.Log (myPlayer.name);


	}




	// Update is called once per frame
	void Update () {
		#if !UNITY_EDITOR
		if (!GetComponent<PhotonView> ().isMine) {
			Transform cam = Camera.current.transform;
			if (playerNameTag != null && cam != null) {
				playerNameTag.LookAt (cam);
				playerNameTag.Rotate (0, 180, 0);
			}
		}
	
	
		#endif

		if (GetComponent<PhotonView> ().isMine) {
			if (Health <= 0) {
				//kondisi ketika player mati
				if (spawnPos == null) {
					gameObject.SetActive (false);
				} else {
					transform.position = spawnPos.transform.position;
					MyColor = new Color (1, 1, 1, 1);
				}
			}
		}
			
		
	}

	public Vector3 GetPointShoot(){
		
		RaycastHit hit;


		Vector3 near = new Vector3 (Screen.width/2.0f , Screen.height / 2.0f, cam.GetComponent<Camera> ().nearClipPlane);
		Vector3 far = new Vector3 (Screen.width/2.0f, Screen.height / 2.0f, cam.GetComponent<Camera> ().farClipPlane);
		near = cam.GetComponent<Camera> ().ScreenToWorldPoint (near);
		far = cam.GetComponent<Camera> ().ScreenToWorldPoint (far);
		Ray ry = new Ray (near, far - near);
		Debug.DrawRay(near, far - near);
		if (Physics.Raycast (ry , out hit , 1000) ){
			Debug.Log (hit.transform.name);
			return hit.point;
		}

		return transform.GetChild(0).transform.forward * 100;
	}

	[PunRPC]
	void CreateMap(int nodeTim){
		if (myNode == null) {

			switch (nodeTim) {
			case 1:
			
				myNode = Instantiate (nodePlayerBlue, transform.position, Quaternion.identity, transform);
				break;
			case 2:
				
				myNode = Instantiate (nodePlayerRed, transform.position, Quaternion.identity, transform);
				break;
			case 3:
				
				myNode = Instantiate (nodeGreen, transform.position, Quaternion.identity, transform);
				break;
	
			}
		} 
	
	}

	void OnTriggerEnter(Collider col){
		if (col.transform.tag == "Tim") {
			MyColor = col.GetComponent<Renderer> ().material.color;
			if (GetComponent<PhotonView> ().isMine) {
				if (MyColor.b == 1) {
				
					mapCameraRenderer.GetComponent<Camera> ().cullingMask = GameManagerScript.MapBlueTim;

					GetComponent<PhotonView> ().RPC ("CreateMap", PhotonTargets.Others, 1);
				
				} else if (MyColor.r == 1) {
					
					mapCameraRenderer.GetComponent<Camera> ().cullingMask = GameManagerScript.MapRedTim;
					GetComponent<PhotonView> ().RPC ("CreateMap", PhotonTargets.Others, 2);

				}
				CreateMap (3);
			}

		}

	}
	void OnCollisionEnter(Collision col){

		if (col.transform.tag == "Bullet" ) {
			Debug.Log (col.transform.GetComponent<Renderer> ().material.color != MyColor);
			if(col.transform.GetComponent<Renderer> ().material.color.r != MyColor.r){
				float dmg = col.transform.GetComponent<Bullet> ().Damage;
				GetDamage (dmg);
				Destroy (col.gameObject);
				Debug.Log (Health);
			}

		}

	}
		
}
