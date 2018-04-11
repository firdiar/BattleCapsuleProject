using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatListingLayoutGroup : MonoBehaviour {

	[SerializeField]GameObject ChatInstantiate = null;
	PhotonView pv;
	List<ChatListing> _allChat = new List<ChatListing>();
	List<ChatListing> AllChats{
		get{ return _allChat; }
	}


	void Awake(){
		pv = GetComponent<PhotonView> ();
	}

	public void FlowUp(string isi , string sender){
		pv.RPC ("SummonChat", PhotonTargets.All, isi, sender);

	}

	void OnPhotonSerializeView(PhotonStream stream , PhotonMessageInfo info){
		
	}



	[PunRPC]
	private void SummonChat(string isi , string sender){

		GameObject temp = Instantiate (ChatInstantiate);
		temp.transform.SetParent (this.transform);
		temp.transform.localScale = Vector3.one;
		temp.transform.localPosition = new Vector3(temp.transform.localPosition.x , temp.transform.localPosition.y , 0);
		temp.transform.localRotation = Quaternion.identity;

		ChatListing cl = temp.GetComponent<ChatListing>();
		cl.FlowUpChat(isi , sender);

		AllChats.Add(cl);

	}
	void FixedUpdate(){
		if (AllChats.Count > 15) {
			Destroy (AllChats [0].gameObject);
			AllChats.RemoveAt (0);
		}

	}
}
