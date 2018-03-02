using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

public class MenuLoginManager : MonoBehaviour {
	[Header("Login Page")]
	public UnityEngine.UI.InputField emailTxtL;
	public UnityEngine.UI.InputField passwordTxtL;
	public UnityEngine.UI.Text errorTxtL;

	[Header("Register Page")]
	public UnityEngine.UI.InputField nickNameTxtR;
	public UnityEngine.UI.InputField emailTxtR;
	public UnityEngine.UI.InputField passwordTxtR;
	public UnityEngine.UI.InputField confirmPasswordTxtR;
	public UnityEngine.UI.Text errorTxtR;

	[SerializeField]bool isLoginPage = true;
	[SerializeField]GameObject loginPage = null;
	[SerializeField]GameObject regisPage = null;

	[Header("Loading UI")]
	[SerializeField]GameObject loadingScene = null;

	[Header("Loby Scene")]
	[SerializeField] string LobyScene = "";

	public delegate void ContinueTo();
	public static event ContinueTo gotoLoby;

	void OnEnable(){
		gotoLoby += GotoLoby;
	}
	void OnDisable(){
		gotoLoby -= GotoLoby;
	}


	public void OnClickLogin_Btn(UnityEngine.UI.Button btn){
		//btn.interactable = false;
		if (emailTxtL.text == "") {
			errorTxtL.text = "Nickname cant blank";
			return;
		} else if (emailTxtL.text == "") {
			errorTxtL.text = "Email cant't blank";
			return;
		} 
		SetActiveLoadingScene (true);
		//string result = "";

		FirebaseHandlerScriptManager.Sign (emailTxtL.text, passwordTxtL.text, errorTxtL, loadingScene , gotoLoby);

	}

	public void OnClickRegister_Btn(){
		FirebaseHandlerScriptManager.LogOut ();

		//aturan untuk daftar player
		if (nickNameTxtR.text == "") {
			errorTxtR.text = "Nickname cant blank";
			return;
		} else if (emailTxtR.text == "") {
			errorTxtR.text = "Email cant't blank";
			return;
		} else if (passwordTxtR.text == "") {
			errorTxtR.text = "Password Can't blank";
			return;
		}else if(nickNameTxtR.text.Length < 6){
			errorTxtR.text = "Nickname Min Have 6 Character";
			return;
		}else if(passwordTxtR.text.Length < 6){
			errorTxtR.text = "Password Min Have 6 Character";
			return;
		}else if(nickNameTxtR.text == passwordTxtR.text){
			errorTxtR.text = "Nickname Must'nt same with Password";
			return;
		}else if (passwordTxtR.text != confirmPasswordTxtR.text) {
			errorTxtR.text = "Password Not Same";
			return;
		}else if (!emailTxtR.text.Contains ("@") || !(emailTxtR.text.Contains(".com") || emailTxtR.text.Contains(".co.id")) ) {
			errorTxtR.text = "Wrong Email or not Support Email";
			return;
		}
		SetActiveLoadingScene (true);
		FirebaseHandlerScriptManager.RegisterNewAccount (emailTxtR.text, passwordTxtR.text, nickNameTxtR.text , errorTxtR ,loadingScene , gotoLoby);
		//FirebaseHandlerScriptManager.SendEmailVerification (errorTxtR);

	}

	public void ChangeLoginPage(bool page){
		isLoginPage = page;
	}

	public void ReSendEmailVerivication(UnityEngine.UI.Text test){
		FirebaseHandlerScriptManager.SendEmailVerification (test);
	}

	void Start(){
		if (FirebaseHandlerScriptManager.auth.CurrentUser != null) {
			Input.compass.enabled = true;
			GotoLoby ();

		}

	}

	void GotoLoby(){
		AsyncOperation progress = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (LobyScene);
		StartCoroutine (ChangeScene (progress));
	}

	void SetActiveLoadingScene(bool val){
		loadingScene.SetActive (val);
	}

	IEnumerator ReInteracButton(UnityEngine.UI.Button btn , float time){
		yield return new WaitForSeconds (time);
		btn.interactable = true;
	
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

	void Update(){

		if (isLoginPage) {
			if (loginPage.activeInHierarchy)
				return;
			errorTxtR.text = "";
			loginPage.SetActive (true);
			regisPage.SetActive (false);
		} else {
			if (regisPage.activeInHierarchy)
				return;
			errorTxtL.text = "";
			loginPage.SetActive (false);
			regisPage.SetActive (true);
		
		}
	}


}
