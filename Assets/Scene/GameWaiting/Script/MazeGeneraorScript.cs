using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneraorScript : MonoBehaviour {

	public class WallCell
	{
		public bool isVisited = false;
		GameObject _wallDepan;
		GameObject _wallKanan;
		GameObject _wallBlakang;
		GameObject _wallKiri;

		public Vector3 PositionCell {
			get;
			set;
		}

		public GameObject WallDepan{
			get{ return _wallDepan;}
			set{ _wallDepan = value; }
		}
		public GameObject WallBlakang{
			get{ return _wallBlakang;}
			set{ _wallBlakang = value; }
		}
		public GameObject WallKanan{
			get{ return _wallKanan;}
			set{ _wallKanan = value; }
		}
		public GameObject WallKiri{
			get{ return _wallKiri;}
			set{ _wallKiri = value; }
		}
		public static void DestroyWall(GameObject obj){

			Destroy(obj);
		}

	}


	public bool createOnStart {
		private get;
		set;
	}

	public GameObject wall;
	public int sizeX;
	public int sizeY;
	public float panjangWall;
	public Vector3 startPos;
	[Range(0.0001f , 1f)]
	[SerializeField]float delayMakeLabyrith = 0;
	GameObject MazeParent;

	PhotonView pv;
	WallCell[,] arrWalls;
	GameObject tempWall;
	int maze;

	// Use this for initialization
	void Start () {
		Debug.Log("Masuk");
		maze = 0;
		pv = GetComponent<PhotonView> ();

		
		arrWalls = new WallCell[sizeY, sizeX];
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				//Debug.Log (i + " , " + j);
				arrWalls [j , i] = new WallCell ();
			
			}
		
		}
		StartCreateMaze ();

//		Debug.Log (arrWalls [3, 1].WallBlakang.name);
//		Debug.Log (arrWalls [3, 1].WallDepan.name);
//		Debug.Log (arrWalls [3, 1].WallKanan.name);
//		Debug.Log (arrWalls [3,1].WallKiri.name);
	}



	//Membuat Maze
	public void StartCreateMaze(){
		MazeParent = new GameObject ();
		MazeParent.name = "MazeEnvironment";
		CreateBlankMaze ();
		if (PhotonNetwork.isMasterClient==false)
			return;

		Debug.Log("Created");
		StartCoroutine(CreateWalkThrough (0 , 0));
		MazeParent.transform.SetParent (GameObject.Find ("Environment").transform);

	
	}
	
	//Membuat maze awal
	void CreateBlankMaze(){
		Vector3 posAwal = startPos;
		if (maze >= 1)
			return;

		maze += 1;
		Debug.Log ("Membuat Blank Maze");

		//Membuat Wall Kanan / Kiri
		for (int i = 0; i <= sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				
				tempWall = (GameObject)Instantiate (wall , posAwal + new Vector3( j*panjangWall + panjangWall/2, 0 , i*panjangWall ) , Quaternion.identity );
				tempWall.name = "Wall " + j.ToString() + " , " + i.ToString () + " Hor";
				if (i < sizeX && j < sizeY && tempWall != null) {
					arrWalls [j, i].WallKiri = tempWall;
					if (i > 0) {
						arrWalls [j, i - 1].WallKanan = tempWall;
					}

				} else {
					arrWalls [j, i - 1].WallKanan = tempWall;
				
				}
				tempWall.transform.SetParent( MazeParent.transform);
			}
		}

		//Membuat Wall Depan / Blakang
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j <= sizeY; j++) {

				tempWall = (GameObject)Instantiate (wall , posAwal + new Vector3( j*panjangWall , 0 , i*panjangWall + panjangWall/2 ) , Quaternion.Euler(0 , 90 , 0));
				tempWall.name = "Wall " + j.ToString() + " , " + i.ToString ()+" Ver";
				if (i < sizeX && j < sizeY && tempWall != null) {
					arrWalls [j, i].WallDepan = tempWall;
					if (j > 0) {
						arrWalls [j - 1, i].WallBlakang = tempWall;
					}

				} else {
					arrWalls [j - 1, i].WallBlakang = tempWall;
				}
				tempWall.transform.SetParent( MazeParent.transform);
			}
		}

		foreach (WallCell c in arrWalls) {
			c.PositionCell = new Vector3 (c.WallDepan.transform.position.x - (c.WallDepan.transform.position.x - c.WallBlakang.transform.position.x)/2, 0 , c.WallDepan.transform.position.z );
			//Debug.Log (c.PositionCell);
			//Instantiate (wall, c.PositionCell, Quaternion.identity);
		}


	}

	/// <summary>
	/// Creates the walk throught for labiryth
	/// </summary>
	/// <param name="baris">baris saat ini</param>
	/// <param name="kolom">Kolom saat ini</param>
	IEnumerator CreateWalkThrough (int kolom = 0, int baris = 0){

		if (maze < 2) {
			

			string walkThrough = GetNextPath (kolom, baris);
			List<Vector2> listStep = new List<Vector2> ();

			while (walkThrough != "Clear") {
				//Debug.Log (walkThrough);
				arrWalls [kolom, baris].isVisited = true;
				Vector2 nextStep = Vector2.zero;
				listStep.Clear ();
				string[] jalan = walkThrough.Split (',');


				foreach (string isi in jalan) {
					if (isi != " ") {
						string[] xy = isi.Split ('.');
						listStep.Add (new Vector2 (System.Convert.ToInt32 (xy [0]), System.Convert.ToInt32 (xy [1])));
					}
				}
				int count = listStep.Count;

				int random = Random.Range (0, count);
				//Debug.Log (random);

				nextStep = listStep [random];
				//Debug.Log (listStep.Count+"  "+random +"   " + nextStep);
				if (nextStep.x != kolom) {
					if (nextStep.x > kolom) {
						//Destroy(arrWalls[kolom , baris].WallBlakang);
						//DestroyWallNumber(kolom, baris , 3);
						pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 3);
					} else {
						//Destroy(arrWalls[kolom , baris].WallDepan);
						//DestroyWallNumber(kolom, baris , 1);
						pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 1);
					}
			
				} else if (nextStep.y != baris) {
					if (nextStep.y > baris) {
						//Destroy(arrWalls[kolom , baris].WallKanan);
						//DestroyWallNumber(kolom, baris , 2);
						pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 2);
					} else {
						//Destroy(arrWalls[kolom , baris].WallKiri);
						//DestroyWallNumber(kolom, baris , 4);
						pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 4);
					}
			
				}

				kolom = (int)nextStep.x;
				baris = (int)nextStep.y;
				arrWalls [kolom, baris].isVisited = true;



				walkThrough = "";
				walkThrough = GetNextPath (kolom, baris);
				yield return new WaitForSeconds (delayMakeLabyrith);
				//Debug.Log (walkThrough);
				if (walkThrough == "Clear") {
					bool recreate = false;
					RecheckMap (ref kolom, ref baris, ref recreate);
					if (recreate) {
						walkThrough = GetNextPath (kolom, baris);
					} else {
						break;
					}
				}

			}
		}

	
	}
	public void DestroyRandom(int i){
		if (i <= 0)
			return;


		int kolom = Random.Range (0, sizeY);
		int baris = Random.Range (0, sizeX);
		bool isDestroying = false;
		string[] result = CekWallExist (kolom, baris);
		if (result [0]=="1") {
			pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 1);
			isDestroying = true;
		}
		if (result [1]=="1") {
			pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 2);
			isDestroying = true;
		}
		if (result [2]=="1") {
			pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 3);
			isDestroying = true;
		}
		if (result [3]=="1") {
			pv.RPC ("DestroyWallNumber", PhotonTargets.All, kolom, baris, 4);
			isDestroying = true;
		}

		if (isDestroying == false) {
			DestroyRandom (i-1);
		}

	}

	string[] CekWallExist(int kolom , int baris){
		string[] hasil = new string[4];
		
		if (arrWalls [kolom, baris].WallDepan != null && (kolom != 0))
			hasil [0] = "1";
		else
			hasil [0] = "0";

		if (arrWalls [kolom, baris].WallKanan != null && (baris != sizeX-1))
			hasil [1] = "1";
		else
			hasil [1] = "0";

		if (arrWalls [kolom, baris].WallBlakang != null &&(kolom != sizeY-1))
			hasil [2] = "1";
		else
			hasil [2] = "0";

		if (arrWalls [kolom, baris].WallKiri != null && (baris != 0))
			hasil [3] = "1";
		else
			hasil [3] = "0";

		return hasil;
	}


	[PunRPC]
	void DestroyWallNumber(int kolom , int baris , int pos){
		//Debug.Log (kolom + "  " + baris + "  " + pos);
		switch (pos) {

		case 1:
			WallCell.DestroyWall(arrWalls[kolom , baris].WallDepan);
			//Destroy(arrWalls[kolom , baris].WallDepan);
			//Debug.Log (arrWalls [kolom, baris].WallDepan.name);
			break;
		case 2:
			//Debug.Log (arrWalls[kolom , baris].WallKanan.gameObject.name);
			WallCell.DestroyWall(arrWalls[kolom , baris].WallKanan);
			//Destroy(arrWalls[kolom , baris].WallKanan);
			break;
		case 3:
			//Debug.Log (arrWalls[kolom , baris].WallBlakang.gameObject.name);
			WallCell.DestroyWall(arrWalls[kolom , baris].WallBlakang);
			//Destroy(arrWalls[kolom , baris].WallBlakang);
			break;
		case 4:
			//Debug.Log (arrWalls[kolom , baris].WallKiri.gameObject.name);
			WallCell.DestroyWall(arrWalls[kolom , baris].WallKiri);
			//Destroy(arrWalls[kolom , baris].WallKiri);
			break;

		}

	}

	void RecheckMap (ref int kolom, ref int baris , ref bool recreate){
		bool belumTerlewat = false;

		for (int i = sizeX - 1; i >= 0; i--) {

			for (int j = sizeY - 1; j >= 0 ; j--) {
				//Debug.Log (i + " , " + j);
				//Debug.Log(arrWalls [j , i].isVisited == false);
				if(arrWalls [j , i].isVisited == false){
					if (!belumTerlewat) {
						belumTerlewat = true;
						if (i + 1 < sizeX) {
							kolom = j;
							baris = i + 1;
							recreate = true;
							return;

						}
					}
				}
				if (belumTerlewat && arrWalls [j, i].isVisited == true) {
					kolom = j;
					baris = i;
					string walkTemp = GetNextPath (kolom, baris);
					if (walkTemp == "Clear") {
						continue;
					}
					recreate = true;
					return;
				}
			}
		}

		recreate = false;

	
	}

	string GetNextPath(int kolom, int baris){
		string atas = " ";
		string bawah = " ";
		string kanan = " ";
		string kiri = " ";

		if ( (kolom - 1 >= 0) && arrWalls [kolom - 1, baris].isVisited == false) {
			atas = (kolom - 1).ToString()+"."+baris.ToString();
		}

		if ( (kolom + 1 < sizeY) && arrWalls [kolom + 1, baris].isVisited == false) {
			bawah = (kolom + 1).ToString()+"."+baris.ToString();
		}
		if ( (baris - 1 >= 0) && arrWalls [kolom, baris  - 1].isVisited == false) {
			kiri = kolom.ToString()+"."+(baris - 1).ToString();
		}
		if ( (baris + 1 < sizeX) && arrWalls [kolom, baris  + 1].isVisited == false) {
			kanan = kolom.ToString()+"."+(baris + 1).ToString();
		}

		if ((atas == " ") &&(bawah == " ")&&(kanan == " ") && (kiri == " ")) {
			return "Clear";
		}

	
		return atas + "," + bawah + "," + kanan + "," + kiri;
	}


}
