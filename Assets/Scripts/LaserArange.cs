using UnityEngine;
using System.Collections;

public class LaserArange : MonoBehaviour {

	SphereCollider sc;
	LineRenderer aline;
	LaserShooter ls;
	UnitControl uc;

	float radius;
	int segments;

	// Use this for initialization
	void Start () {
		aline = gameObject.AddComponent<LineRenderer> ();
		ls = transform.parent.GetComponent<LaserShooter>();
		sc = transform.GetComponent<SphereCollider> ();

		uc = (UnitControl)transform.parent.GetComponent (typeof(UnitControl));
		
		if(uc != null)
			setAttackRange (sc, uc.getName());

		settingCircle ();
		createPoints ();
	}
	
	// Update is called once per frame
	void Update () {
		if (uc.selected) {
			aline.enabled = true;
		} else {
			aline.enabled = false;
		}
	}

	void OnTriggerEnter(Collider other){
		
	}
	
	void OnTriggerStay(Collider other){
		//		if (ec != null) {
		//			if (other.tag == "Player") {
		//				ec.attackRotation (other.gameObject.transform.position);
		//			}
		//		}
		
		if (uc != null) {
			if (other.tag == "Enemy") {
				uc.attackRotation (other.gameObject.transform.position, "laser");
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		
	}

	void createPoints(){
		float x, y = 0f, z = 0f;
		float angle = 20f;
		
		aline.SetVertexCount (segments + 1);
		aline.useWorldSpace = false;
		aline.material = new Material (Shader.Find ("Particles/Additive"));
		aline.SetColors (new Color(0.5f, 0.5f, 0.5f, 0.5f), new Color(0.5f, 0.5f, 0.5f, 0.5f));
		
		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			
			aline.SetPosition(i, new Vector3(x, y, z));
			
			angle += (360f / segments);
		}
	}

	void setAttackRange(SphereCollider arangeC, string tname){
		if (tname == "tank") {
			arangeC.radius = 30.0f;
		} else if(tname == "bomber"){
			arangeC.radius = 32.0f;
		} else if (tname == "cube") {
			arangeC.radius = 15.2f;
		} else if(tname == "sphere"){
			arangeC.radius = 17.6f;
		} else if(tname == "cylinder"){
			arangeC.radius = 22.0f;
		} else if(tname == "enemy_001"){
			arangeC.radius = 15.2f;
		} else {
			arangeC.radius = 15.0f;
		}
	}

	void settingCircle(){
		radius = sc.radius;

		if (radius < 10.0f) {
			segments = 60;
		} else if (radius < 14.0f) {
			segments = 80;
		} else if (radius < 18.0f) {
			segments = 100;
		} else {
			segments = 120;
		}
	}
}
