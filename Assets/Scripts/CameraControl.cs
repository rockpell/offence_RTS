using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public float movement = 2.0f;
	public Camera camera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
//			transform.position += new Vector3(-movement, 0, 0);
			transform.Translate((Vector3.left * movement) * Time.deltaTime * 100);
		}
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
//			transform.position += new Vector3(movement, 0, 0);
			transform.Translate((Vector3.right * movement) * Time.deltaTime * 100);
		}
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
//			transform.position += new Vector3(0, 0, movement);
			transform.Translate((Vector3.forward * movement) * Time.deltaTime * 100);
		}
		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
//			transform.position += new Vector3(0, 0, -movement);
			transform.Translate((Vector3.back * movement) * Time.deltaTime * 100);
		}
	}
}
