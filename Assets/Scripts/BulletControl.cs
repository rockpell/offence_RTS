using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {

	public float speed = 5.0f;

	Vector3 targetDiretion;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (targetDiretion != new Vector3(0, 0, 0)) {
			transform.position += targetDiretion * speed;
			Debug.Log("update");
		}
		Debug.Log ("target : " + targetDiretion);
	}

	public void setDirection(Vector3 thisPosition, Vector3 targetPosition){
		Vector3 result = targetPosition - thisPosition;
		targetDiretion = result.normalized;
		Debug.Log (targetDiretion);
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "Cube2") {
			Object.Destroy(this.gameObject);
		}
	}
}
