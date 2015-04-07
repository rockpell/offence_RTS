using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public float movement = 2.0f;
	public Camera camera;
	public RectTransform rectBox;

	UnitSystem us;

	Vector3 boxPosition;

	float boxWidth, boxHeight;

	// Use this for initialization
	void Start () {
		us = UnitSystem.instance;
		boxPosition = rectBox.transform.position;
		boxWidth = rectBox.rect.width;
		boxHeight = rectBox.rect.height;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && transform.position.x > boxPosition.x - boxWidth/2) {
//			transform.position += new Vector3(-movement, 0, 0);
			transform.Translate((Vector3.left * movement) * Time.deltaTime * 100);
		}
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && transform.position.x < boxPosition.x + boxWidth/2){
//			transform.position += new Vector3(movement, 0, 0);
			transform.Translate((Vector3.right * movement) * Time.deltaTime * 100);
		}
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && transform.position.z < boxPosition.z + boxHeight/2){
//			transform.position += new Vector3(0, 0, movement);
			transform.Translate((Vector3.forward * movement) * Time.deltaTime * 100);
		}
		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && transform.position.z > boxPosition.z - boxHeight/2){
//			transform.position += new Vector3(0, 0, -movement);
			transform.Translate((Vector3.back * movement) * Time.deltaTime * 100);
		}

		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && camera.fieldOfView > 51) {
			camera.fieldOfView -= 1;
			us.sizeContorl("up");
		}

		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && camera.fieldOfView < 76) {
			camera.fieldOfView += 1;
			us.sizeContorl("down");
		}
	}
}
