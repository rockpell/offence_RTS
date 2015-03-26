using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	GameObject camera1;

	// Use this for initialization
	void Start () {
		camera1 = GameObject.Find ("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		float translationX = Input.GetAxis ("Horizontal");
		float translationY = Input.GetAxis ("Vertical");
		float fastTranslationX = 2 * translationX;
		float fastTranslationY = 2 * translationY;

		if (Input.GetKey (KeyCode.LeftShift)) {
			transform.Translate(fastTranslationX + fastTranslationY, 0, fastTranslationY - fastTranslationX);
		} else {
			transform.Translate(translationX + translationY, 0, translationY - translationX);
		}

		float mousePosX = Input.mousePosition.x;
		float mousePosY = Input.mousePosition.y;
		float scrollSpeed = 70f;
		int scrollDistance = 5;

		if(mousePosX < scrollDistance){
			transform.Translate(-1, 0, 1);
		}
		if(mousePosX >= Screen.width - scrollDistance){
			transform.Translate(1, 0, -1);
		}
		if(mousePosY < scrollDistance){
			transform.Translate(-1, 0, -1);
		}
		if (mousePosY >= Screen.height - scrollDistance) {
			transform.Translate(1, 0, 1);
		}


		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && camera1.camera.orthographicSize > 200) {
			camera1.camera.orthographicSize -= 4; 
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && camera1.camera.orthographicSize < 500) {
			camera1.camera.orthographicSize += 4;
		}


		if(Input.GetKeyDown(KeyCode.Mouse2)){
			camera1.camera.orthographicSize = 400;
		}
	}
}
