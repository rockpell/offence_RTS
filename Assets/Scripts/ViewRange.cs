using UnityEngine;
using System.Collections;

public class ViewRange : MonoBehaviour {

	UnitControl uc;
	EnemyControl ec;

	LineRenderer aline;

	int segments;
	float radius;

	// Use this for initialization
	void Start () {
		SphereCollider arangeC = gameObject.GetComponent<SphereCollider> ();

//		uc = (UnitControl)transform.GetComponent (typeof(UnitControl));
		uc = (UnitControl)transform.parent.GetComponent (typeof(UnitControl));
		if(uc != null)
			setAttackRange (arangeC, uc.getName());

		if (uc == null) {
			ec = (EnemyControl)transform.parent.GetComponent(typeof(EnemyControl));
			setAttackRange (arangeC, ec.getName());
		}

		aline = gameObject.AddComponent<LineRenderer> ();

		settingACircle (arangeC);
		createPoints ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){

	}

	void OnTriggerStay(Collider other){
//		Debug.Log ("collide object : "+other.name);
		if (ec != null) {
			if (other.tag == "Player") {
				ec.attackRotation (other.gameObject.transform.position);
			}
		}

		if (uc != null) {
			if (other.tag == "Enemy") {
				uc.attackRotation (other.gameObject.transform.position);
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
		aline.SetColors (Color.gray, Color.gray);
		
		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			
			aline.SetPosition(i, new Vector3(x, y, z));
			
			angle += (360f / segments);
		}
	}

	void settingACircle(SphereCollider arangeC){
		radius = arangeC.radius;
	}

	void setAttackRange(SphereCollider arangeC, string tname){
		if (tname == "tank") {
			arangeC.radius = 7.0f;
			segments = 100;
		} else if (tname == "cube") {
			arangeC.radius = 4.2f;
			segments = 80;
		} else if(tname == "sphere"){
			arangeC.radius = 4.6f;
			segments = 80;
		} else if(tname == "cylinder"){
			arangeC.radius = 5.0f;
			segments = 85;
		} else if(tname == "enemy_001"){
			arangeC.radius = 4.2f;
			segments = 80;
		} else {
			arangeC.radius = 3.0f;
			segments = 60;
		}
	}

}
