using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatListing : MonoBehaviour {

	[SerializeField]Text _sender=null;
	private Text Sender{
		get{return _sender;}
	}

	[SerializeField]Text _isi = null;
	public Text Isi{
		get{ return _isi; }
	}


	public void FlowUpChat(string isi , string sender){
		Sender.text = sender;
		Isi.text = isi;
	}
}
