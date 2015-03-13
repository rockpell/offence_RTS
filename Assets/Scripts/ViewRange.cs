using UnityEngine;
using System.Collections;

public class ViewRange : MonoBehaviour {

	UnitControl uc;

	// Use this for initialization
	void Start () {
		uc = (UnitControl)transform.parent.GetComponent (typeof(UnitControl));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){

	}

	void OnTriggerStay(Collider other){
//		Debug.Log ("collide object : "+other.name);
		if (other.name == "Cube2") {
			Debug.Log("cube2");
			uc.attackRotation(other.gameObject.transform.position);
		}
	}

	void OnTriggerExit(Collider other){

	}
}
