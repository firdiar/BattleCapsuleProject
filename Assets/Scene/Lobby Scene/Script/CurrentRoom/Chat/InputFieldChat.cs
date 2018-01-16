using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldChat : MonoBehaviour {

	[SerializeField]InputField textFlowUP=null;

	[SerializeField]ChatListingLayoutGroup tabListing = null;

	public void FlowUp(){
	
		if ((textFlowUP.text != "")&&(textFlowUP.text.Length <= 120)) {
			string isi = textFlowUP.text;
			textFlowUP.text = "";
			tabListing.FlowUp (isi, PhotonNetwork.player.NickName);
		}
	}
}
