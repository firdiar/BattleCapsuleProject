using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEX : MonoBehaviour {

	public RectTransform Main;
	public GameObject rendeem;
	public GameObject DTM;


	// Use this for initialization
	void Awake () {
		Debug.Log (Main.position);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Change(string i){
		Debug.Log (Main.localPosition);

		if (DTM.activeInHierarchy) {
			rendeem.SetActive (true);
			DTM.SetActive(false);

		}else if (rendeem.activeInHierarchy) {
			DTM.SetActive(true);
			rendeem.SetActive (false);
		}
	

		
	}

	public void Convert(int val){
		int currentDiamond = MainCanvasManager.Instance.LobbyCanvas.GetComponent<LobbyCanvas> ().GetDiamond ();
		int currentCoin = MainCanvasManager.Instance.LobbyCanvas.GetComponent<LobbyCanvas> ().GetGold ();

		if (val <= currentDiamond) {
			currentDiamond -= val;

			if (val > 500)
				val += 150;
			else if (val > 300)
				val += 70;
			else if (val > 150)
				val += 25;
			
			FirebaseHandlerScriptManager.SetDiamond (currentDiamond.ToString ());
			FirebaseHandlerScriptManager.SetCoin ((currentCoin+(val*3)).ToString ());
		}
	}

	public void Rendeem(UnityEngine.UI.InputField Input){
		int currentDiamond = MainCanvasManager.Instance.LobbyCanvas.GetComponent<LobbyCanvas> ().GetDiamond ();
		FirebaseHandlerScriptManager.RendeemDiamond (Input.text , MainCanvasManager.Instance.loadingPrefabs , currentDiamond);
	}
}
