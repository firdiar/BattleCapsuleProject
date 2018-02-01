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
	[SerializeField]float delayMakeLabyrith;
	GameObject MazeParent;


	WallCell[,] arrWalls;
	GameObject tempWall;


	// Use this for initialization
	void Awake () {
		
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

		StartCoroutine(CreateWalkThrough (0 , 0));
		MazeParent.transform.SetParent (GameObject.Find ("Environment").transform);

	
	}
	
	//Membuat maze awal
	void CreateBlankMaze(){
		Vector3 posAwal = startPos;


		//Membuat Wall Kanan / Kiri
		for (int i = 0; i <= sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				
				tempWall = (GameObject)Instantiate (wall , posAwal + new Vector3( j*panjangWall + panjangWall/2, 0 , i*panjangWall ) , Quaternion.identity);
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

				tempWall = (GameObject) Instantiate (wall , posAwal + new Vector3( j*panjangWall , 0 , i*panjangWall + panjangWall/2 ) , Quaternion.Euler(0 , 90 , 0));
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


	}

	/// <summary>
	/// Creates the walk throught for labiryth
	/// </summary>
	/// <param name="baris">baris saat ini</param>
	/// <param name="kolom">Kolom saat ini</param>
	IEnumerator CreateWalkThrough (int kolom = 0, int baris = 0){


		string walkThrough = GetNextPath (kolom , baris);
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
					listStep.Add (new Vector2 (System.Convert.ToInt32( xy [0]), System.Convert.ToInt32( xy [1]) ));
				}
			}
			int count = listStep.Count;

			int random = Random.Range (0 , count);
			//Debug.Log (random);

			nextStep = listStep[random];
			//Debug.Log (listStep.Count+"  "+random +"   " + nextStep);
			if (nextStep.x != kolom) {
				if (nextStep.x > kolom) {
					Destroy(arrWalls[kolom , baris].WallBlakang);
				} else {
					Destroy(arrWalls[kolom , baris].WallDepan);
				}
			
			} else if (nextStep.y != baris) {
				if (nextStep.y > baris) {
					Destroy(arrWalls[kolom , baris].WallKanan);
				} else {
					Destroy(arrWalls[kolom , baris].WallKiri);
				}
			
			}

			kolom = (int)nextStep.x;
			baris = (int)nextStep.y;
			arrWalls [kolom, baris].isVisited = true;



			walkThrough = "";
			walkThrough = GetNextPath (kolom , baris);
			yield return new WaitForSeconds (delayMakeLabyrith);
			Debug.Log (walkThrough);
			if (walkThrough == "Clear") {
				bool recreate = false;
				RecheckMap (ref kolom, ref baris , ref recreate);
				if (recreate) {
					walkThrough = GetNextPath (kolom , baris);
				} else {
					break;
				}
			}

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
