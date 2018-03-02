using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {
	[SerializeField]GameObject[] spawnPlayerPos = null;
	[SerializeField]Text timeText = null;
	[SerializeField]BaseScript timA = null;
	[SerializeField]BaseScript timB = null;


	public LayerMask MaskBlueTim;
	public LayerMask MaskRedTim;
	public static LayerMask MapBlueTim;
	public static LayerMask MapRedTim;

	public GameObject message;
	Text messege_Text;
	public GameObject PanelMask;
	public GameObject loadingScene;
	[SerializeField]string LobyScene = "";

	List<GameObject> allPlayer = new List<GameObject>();
	int playerInScene;
	public int playerInRoom;
	bool summon = true;
	bool colliderActive = false;
	int timeToTxt;
	float time;

	// Use this for initialization
	void Awake () {
		time = 20;
		summon = true;

		playerInRoom = PhotonNetwork.playerList.Length;
		messege_Text = message.transform.GetChild (0).GetComponent<Text> ();
		PhotonNetwork.RPC (GetComponent<PhotonView> (), "JoinedScene", PhotonTargets.MasterClient, false);
		message.SetActive (false);

		Debug.Log (playerInRoom);
		MapBlueTim = MaskBlueTim;
		MapRedTim = MaskRedTim;


	}

	void SetPanelMask(bool c){
		PanelMask.SetActive (c);
	}

	[PunRPC]
	void JoinedScene(){
		playerInScene += 1;
		Debug.Log (playerInScene);

	
	}
		
	[PunRPC]
	void SummonCharacter(){
		GameObject spawnpos = spawnPlayerPos [Random.Range (0, spawnPlayerPos.Length - 1)];
		GameObject character = PhotonNetwork.Instantiate ("Player", spawnpos.transform.position, Quaternion.identity, 0);
		character.GetComponent<PlayerActiveController> ().spawnPos = spawnpos;
		summon = false;
		InvokeRepeating ("CheckPlayerLeft", 30 , 4);
		messege_Text.text = "Karakter Telah Terbuat \n" + "Pilih Tim Dengan Keluar Melalui 2 jalan yang ada";
		message.SetActive (true);	
		Debug.Log ("Karakter terbuat");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			PanelMask.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2 (205 , 61);
			PanelMask.SetActive (!PanelMask.activeInHierarchy);
		}
		
		if ((playerInRoom == playerInScene) && summon) {
			Debug.Log ("Karakter dibuat");
			PhotonNetwork.RPC (GetComponent<PhotonView> (), "SummonCharacter", PhotonTargets.All, false);


			if (PhotonNetwork.isMasterClient) {
				GetComponent<MazeGeneraorScript> ().StartCreateMaze ();
			}

		} else if( !summon && !colliderActive) {
			TimerDown ();
			if (timeToTxt == 0) {
				timA.ActiveCollider ();
				timB.ActiveCollider ();
				colliderActive = true;
				messege_Text.text = "Game Dimulai!!\nWall akan hancur setiap 25 detik";
				GameObject[] tempAllPlayer = GameObject.FindGameObjectsWithTag ("Player");
				for (int i = 0; i < tempAllPlayer.Length; i++) {
					allPlayer.Add (tempAllPlayer [i]);
				}

				StartCoroutine(SetActive(message , false , 3));
			}
		}else if (colliderActive) {
			if (timeToTxt == 0) {
				time = 25;	
				timeToTxt = 25;
				if(PhotonNetwork.isMasterClient)
					GetComponent<MazeGeneraorScript> ().DestroyRandom (4);
			} else {
				TimerDown ();
			}
		
		}

	}

	void TimerDown(){
		time = Mathf.Clamp( time - Time.deltaTime , 0 , time);

		timeToTxt = Mathf.RoundToInt (time);
		timeText.text = timeToTxt.ToString ();
	}
	IEnumerator SetActive(GameObject c , bool status , float Time){
		yield return new WaitForSeconds (time);
		c.SetActive(status);
	}

	void CheckPlayerLeft(){
		int countRed = 0 ;
		int countBlue = 0;
		if (allPlayer.Count >= 1) {
			for (int i = (allPlayer.Count - 1); i >= 0 ; i--) {
				if (allPlayer [i] == null) {
					allPlayer.RemoveAt (i);
					continue;
				} 

				Color c = allPlayer [i].transform.GetChild (1).GetComponent<Renderer> ().material.color;
				if(c == new Color (1, 1, 1, 1) && allPlayer.Count > 1) {
					allPlayer.RemoveAt (i);
				} else if (c.r == 1) {
						countRed++;
				} else if (c.b == 1) {
						countBlue++;
				}

			}
		} 
		Debug.Log (" Jumlah Blue : " + countBlue + "  Jumlah Red : " + countRed);
		if (countRed == 0 || countBlue == 0) {
			Color c = allPlayer [0].transform.GetChild (1).GetComponent<Renderer> ().material.color;
			if (c.b == 1) {
				messege_Text.text = "Blue Tim Win!!!";
			} else if (c.r == 1) {
				messege_Text.text = "Red Tim Win!!!";
			} else 	if (c == new Color(1,1,1,1)) {
				messege_Text.text = "Tidak Ada Player Bermain";
			}
			PanelMask.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2 (205 , 61);
			message.SetActive (true);
			PanelMask.SetActive (true);

		} 
	}

	public void GotoLoby(){
		PhotonNetwork.LeaveRoom (true);
		AsyncOperation progress = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (LobyScene);
		StartCoroutine (ChangeScene (progress));
	}

	IEnumerator ChangeScene (AsyncOperation progress){

		while (!progress.isDone) {
			if (!loadingScene.activeInHierarchy) {
				loadingScene.SetActive (true);
			}
			Color c = loadingScene.transform.GetChild (0).GetComponent<UnityEngine.UI.RawImage> ().color;
			c.a = Mathf.Clamp ((255 * progress.progress) + 0.1f , 160, 255);
			loadingScene.transform.GetChild (0).GetComponent<UnityEngine.UI.RawImage> ().color = c;
			yield return new WaitForSeconds(0);
		}



	}
}
