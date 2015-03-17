using UnityEngine;
using System.Collections;

public class ViewRange : MonoBehaviour {

	UnitControl uc;

	LineRenderer aline;

	int segments;
	float radius;

	// Use this for initialization
	void Start () {
		uc = (UnitControl)transform.parent.GetComponent (typeof(UnitControl));
		aline = gameObject.AddComponent<LineRenderer> ();

		settingACircle ();
		createPoints ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){

	}

	void OnTriggerStay(Collider other){
//		Debug.Log ("collide object : "+other.name);
//		if (other.name == "Cube") {
//			Debug.Log("cube");
//			uc.attackRotation(other.gameObject.transform.position);
//		}
		if(other.tag == "Enemy"){
			uc.attackRotation(other.gameObject.transform.position);
		}
	}

	void OnTriggerExit(Collider other){

	}

	void createPoints(){
		float x, y, z = 0f;
		float angle = 20f;
		
		aline.SetVertexCount (segments + 1);
		aline.useWorldSpace = false;
		aline.material = new Material (Shader.Find ("Particles/Additive"));
		aline.SetColors (Color.gray, Color.gray);
		
		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			
			aline.SetPosition(i, new Vector3(x, y, z));
			
			angle += (360f / segments);
		}
	}

	void settingACircle(){
		SphereCollider arangeC = gameObject.GetComponent<SphereCollider> ();
		radius = arangeC.radius;
		segments = 80;
	}

}
