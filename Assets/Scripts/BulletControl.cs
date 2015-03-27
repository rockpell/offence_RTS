using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {

	public float speed = 5.0f;
	public int damage = 10;

	Vector3 targetDiretion;

	string targetName;

	// Use this for initialization
	void Start () {
		Invoke ("dead", 6.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (targetDiretion != new Vector3(0, 0, 0)) {
			transform.position += targetDiretion * speed;
//			Debug.Log("update");
		}
//		Debug.Log ("target : " + targetDiretion);
	}

	void OnTriggerEnter(Collider other){
//		if (other.name == "Cube") {
//			Object.Destroy(this.gameObject);
//		}

		if (other.tag == targetName) {
			other.gameObject.SendMessage ("applayDamage", damage);
			Object.Destroy(this.gameObject);
		}

//		if(other.tag == "Player"){
//			other.gameObject.SendMessage ("applayDamage", damage);
//			Object.Destroy(this.gameObject);
//		}
	}

	void dead(){
		Destroy (gameObject);
	}

	public void setDirection(Vector3 thisPosition, Vector3 targetPosition){
		Vector3 result = targetPosition - thisPosition;
		targetDiretion = result.normalized;
	}

	public void setTarget(string targetName){
		this.targetName = targetName;
	}
}
