using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroscopeTest : MonoBehaviour {

	float last;
	int doubleLast;
	int tripleLast;
	float lastYRot;
	int tripleLastYRot;
	bool canLookX;
	bool canLookY;
	float i;
	//public float xRotation;
	//public float yRotation;
	void Start(){
		Input.compass.enabled = true;
		//i = 0;
	}

	// Update is called once per frame
	void Update () {
		RotateView ();
		//LookRotation (transform, transform, xRotation , yRotation , 1*5 , 1*5);
	}

	private void RotateView()
	{
		canLookX = false;
		canLookY = false;
		if (!Input.compass.enabled)
			Input.compass.enabled = true;

		float xRotation = Input.compass.magneticHeading;
		//xRotation = Mathf.Floor (xRotation);

		last = transform.localRotation.y;

		if (Mathf.Abs(last - xRotation) > 12) {
			canLookX = true;
			tripleLast = 0;
			doubleLast = 0;

		} else if (last - xRotation > 7) {


			if (doubleLast == 1 && tripleLast != 0) {
				canLookX = true;
				doubleLast = 0;
			} else if (tripleLast == 0) {
				tripleLast = doubleLast;
				doubleLast = 1;
			} else {
				tripleLast = 0;
				doubleLast = 0;
			}


		} else if (xRotation - last > 7) {


			if (doubleLast ==3 && tripleLast != 0) {
				canLookX = true;
				doubleLast = 0;
			} else if (tripleLast == 0) {
				tripleLast = doubleLast;
				doubleLast = 3;
			} else {
				tripleLast = 0;
				doubleLast = 0;
			}


		}else {
			tripleLast = 0;
			doubleLast = 0;
		}





		Vector3 acc = Input.acceleration;
		float yRotation = acc.z + 0.25f;
		lastYRot = transform.localPosition.x / 70;


		yRotation *= 100;
		yRotation = Mathf.Floor (yRotation);
		yRotation = yRotation / 100;

		if (Mathf.Abs (yRotation - lastYRot) > 0.11) {
			canLookY = true;
			tripleLastYRot = 0;
		} else if ((yRotation - lastYRot) > 0.06) {
			if (tripleLastYRot == 1) {
				canLookY = true;
			}
			tripleLastYRot = 1;

		} else if ((lastYRot - yRotation) > 0.06) {
			if (tripleLastYRot == 3) {
				canLookY = true;
			}
			tripleLastYRot = 3;

		} else {
			tripleLastYRot = 0;
		}



		if (canLookX && canLookY) {
			LookRotation (transform, transform, xRotation, yRotation , (tripleLast%2)*5 , (tripleLastYRot%2)*5);
			tripleLastYRot = 0;
			tripleLast = 0;
			//last = xRotation;
			//lastYRot = yRotation;
		} else if (canLookX) {
			LookRotation (transform, transform, xRotation, yRotation, (tripleLast%2)*5 , (tripleLastYRot%2)*5);
			tripleLastYRot = 0;
			//last = xRotation;
		} else if (canLookY) {
			LookRotation (transform, transform, last , yRotation , (tripleLast%2)*5 , (tripleLastYRot%2)*5);
			tripleLast = 0;
			//lastYRot = yRotation;
		}


	}

	public void LookRotation (Transform character, Transform camera, float yRot, float xRot, float timey, float timex)
	{
		xRot = (Mathf.Clamp (xRot, 0.1f, 0.7f)) *70 ;
		//Quaternion m_CharacterTargetRot = Quaternion.Euler (0f, yRot, 0f);
		Quaternion m_CameraTargetRot = Quaternion.Euler ( -xRot , yRot, 0f);


		//character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot, (5 * Time.deltaTime)+timey);
		camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot, 5 * Time.deltaTime);


	}


}
