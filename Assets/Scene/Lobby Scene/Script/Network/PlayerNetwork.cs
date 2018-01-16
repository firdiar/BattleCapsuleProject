using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork instance;
	public string PlayerName{ get; private set;}

	// Use this for initialization
	void Awake () {
		instance = this;

		PlayerName = "Player" + Random.Range (0, 999).ToString();
	}

}
