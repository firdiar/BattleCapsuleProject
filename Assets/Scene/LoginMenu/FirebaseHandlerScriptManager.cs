﻿using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Analytics;

public class FirebaseHandlerScriptManager : MonoBehaviour {

	public static FirebaseHandlerScriptManager instance;
	public static FirebaseAuth auth{ get; set;}
	public static FirebaseUser user;

	public static DatabaseReference rootRefereceDatabase;


	// Use this for initialization
	void Start () {
		auth = FirebaseAuth.DefaultInstance;

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://myfirstunityfirebase.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName("MyFirstUnityFirebase-7b60f011c02b.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("myfirstunityfirebase@appspot.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password("c4ntik");

		rootRefereceDatabase = FirebaseDatabase.DefaultInstance.RootReference;
		instance = this;
		DontDestroyOnLoad (this);
		
	}
		

	public static void RegisterNewAccount(string email , string password , string displayName ,UnityEngine.UI.Text errorText ,GameObject loading){
		auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				loading.SetActive(false);
				errorText.text = "RegisterCanceled";
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				loading.SetActive(false);
				errorText.text = "Register Failed";
				return;
			}

			// Firebase user has been created.
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})",
			newUser.DisplayName, newUser.UserId);


			Firebase.Auth.FirebaseUser user = newUser;

			if (user != null) {
				Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
					DisplayName = displayName,

				};
				user.UpdateUserProfileAsync(profile).ContinueWith(task2 => {
					if (task2.IsCanceled) {
						Debug.LogError("UpdateUserProfileAsync was canceled.");
						return;
					}
					if (task2.IsFaulted) {
						Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
						return;
					}

					Debug.Log("User profile updated successfully.");

					rootRefereceDatabase.Child ("users").Child (user.UserId).Child ("nickname").SetValueAsync (displayName);
					rootRefereceDatabase.Child ("users").Child (user.UserId).Child ("gold").SetValueAsync ("100");
					rootRefereceDatabase.Child ("users").Child (user.UserId).Child ("email").SetValueAsync (user.Email);
					rootRefereceDatabase.Child ("users").Child (user.UserId).Child ("password").SetValueAsync (password);
					rootRefereceDatabase.Child ("users").Child (user.UserId).Child ("diamond").SetValueAsync ("0");

					SendEmailVerification();
					//loading.SetActive(false);
				});
			}


		});
		
	}

	public static void Sign(string email , string password  , UnityEngine.UI.Text errorText , GameObject Loading){

		Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("SignInWithCredentialAsync was canceled.");
				errorText.text = "Login Canceled";
				Loading.SetActive(false);
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
				errorText.text = "Login Failed";
				Loading.SetActive(false);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
			Loading.SetActive(false);

		});
	}

	public static void LogOut(){
		if (auth.CurrentUser != null) {
			auth.SignOut ();
		}
	}

	public static void SendEmailVerification(){

		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		if (user != null) {
			user.SendEmailVerificationAsync().ContinueWith(task => {
				if (task.IsCanceled) {
					Debug.LogError("SendEmailVerificationAsync was canceled.");
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
					return;
				}

				Debug.Log("Email sent successfully.");
			});
		}

	}

	public static void SendEmailVerification(UnityEngine.UI.Text error){

		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		if (user != null) {
			user.SendEmailVerificationAsync().ContinueWith(task => {
				if (task.IsCanceled) {
					Debug.LogError("SendEmailVerificationAsync was canceled.");
					error.text = "SendEmailVerificationAsync was canceled.";
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
					error.text = "SendEmailVerificationAsync encountered an error: " + task.Exception;
					return;
				}

				Debug.Log("Email sent successfully.");
				error.text = "Email sent successfully.";
			});
		}

	}


	public static void SetNewPassword(string password){
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		string newPassword = password;
		if (user != null) {
			user.UpdatePasswordAsync(newPassword).ContinueWith(task => {
				if (task.IsCanceled) {
					Debug.LogError("UpdatePasswordAsync was canceled.");
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError("UpdatePasswordAsync encountered an error: " + task.Exception);
					return;
				}

				Debug.Log("Password updated successfully.");
			});
		}
	}

	public static void SetNewEmail(string email){

		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		if (user != null) {
			user.UpdateEmailAsync(email).ContinueWith(task => {
				if (task.IsCanceled) {
					Debug.LogError("UpdateEmailAsync was canceled.");
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError("UpdateEmailAsync encountered an error: " + task.Exception);
					return;
				}

				Debug.Log("User email updated successfully.");
			});
		}
		
	}

	public static void SendEmailResetVerification(string targetEmail){

		string emailAddress = targetEmail;
		if (user != null) {
			auth.SendPasswordResetEmailAsync(emailAddress).ContinueWith(task => {
				if (task.IsCanceled) {
					Debug.LogError("SendPasswordResetEmailAsync was canceled.");
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
					return;
				}

				Debug.Log("Password reset email sent successfully.");
			});
		}
	}

	public static void DeleteAccount(){

		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		if (user != null) {
			user.DeleteAsync().ContinueWith(task => {
				if (task.IsCanceled) {
					Debug.LogError("DeleteAsync was canceled.");
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
					return;
				}

				Debug.Log("User deleted successfully.");
			});
		}

	}
	

}