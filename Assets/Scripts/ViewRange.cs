using UnityEngine;
using System.Collections;

public class ViewRange : MonoBehaviour {

	UnitControl uc;
	EnemyControl ec;

	LineRenderer aline;

	ArrayList colList;

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

		colList = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
		if (uc != null) {
			if (uc.selected) {
				aline.enabled = true;
			} else {
				aline.enabled = false;
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (uc != null) {
			if (other.tag == "Enemy")
				colList.Add (other.gameObject);
		} else {
			if(other.tag == "Player")
				colList.Add(other.gameObject);
		}
	}

	void OnTriggerStay(Collider other){
//		Debug.Log ("collide object : "+other.name);
		if (ec != null) {
			if (other.tag == "Player") {

				ec.attackRotation (other.gameObject.transform.position);
			}
		}

		if (uc != null) {
			if (other.tag == "Enemy" && other.gameObject.Equals(colList[0])) {
				Vector3 tv = other.gameObject.transform.position;

				if(other.transform.name == "enemy_1"){
					tv.y += 30;
				}

				uc.attackRotation (tv, "view");
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (uc != null) {
			if (other.tag == "Enemy")
				colList.Remove (other.gameObject);
		} else {
			if(other.tag == "Player")
				colList.Remove(other.gameObject);
		}
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

	void settingACircle(SphereCollider arangeC){
		radius = arangeC.radius;
	}

	void setAttackRange(SphereCollider arangeC, string tname){
		if (tname == "tank") {
			arangeC.radius = 10.0f;
			segments = 100;
		} else if(tname == "bomber"){
			arangeC.radius = 12.0f;
			segments = 110;
		} else if (tname == "cube") {
			arangeC.radius = 5.2f;
			segments = 80;
		} else if(tname == "sphere"){
			arangeC.radius = 5.6f;
			segments = 80;
		} else if(tname == "cylinder"){
			arangeC.radius = 6.0f;
			segments = 85;
		} else if(tname == "enemy_001"){
			arangeC.radius = 5.2f;
			segments = 80;
		} else {
			arangeC.radius = 5.0f;
			segments = 60;
		}
	}

}
