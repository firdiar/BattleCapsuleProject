using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork instance = null;
	public string PlayerName{ get; private set;}

	// Use this for initialization
	void Awake () {
		if (PlayerNetwork.instance == null) {
			instance = this;

			PlayerName = PlayerPrefs.GetString ("NickName");
		} else {
			transform.parent.GetComponent<DDOL> ().Destroy ();
		}
	}

}
