using UnityEngine;
using System.Collections;

public class LaserShooter : MonoBehaviour {

	UnitControl uc;

	LineRenderer line;
	RaycastHit[] hit;

	Vector3 targetPosition;

	bool attackPermition = false;

	// Use this for initialization
	void Start () {
		SphereCollider arangeC = gameObject.GetComponent<SphereCollider> ();

		uc = (UnitControl)transform.parent.GetComponent (typeof(UnitControl));

		if(uc != null)
			setAttackRange (arangeC, uc.getName());

		lineSetting ();

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = targetPosition - transform.position;
		float distance = Vector3.Distance(transform.position, targetPosition);

		temp = Vector3.Normalize (temp);
		hit = Physics.RaycastAll (transform.position, temp, distance);

		if (attackPermition) {

			for(var i = 0; i<hit.Length; i++){
				if (hit[i].collider.tag == "Enemy") {
					hit[i].collider.gameObject.SendMessage("applayDamage", 10);
				}
			}

			laserDraw (targetPosition);
			attackPermition = false;
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

	public void laserCall(Vector3 target){
		targetPosition = target;
		attackPermition = true;
	}

	void lineSetting(){
		line = gameObject.AddComponent<LineRenderer> ();
		
		line.SetWidth (1, 1);
		line.SetColors (Color.red, Color.red);
		line.SetVertexCount (2);
		line.useWorldSpace = true;
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.enabled = false;
	}

	void laserDraw(Vector3 target){
		line.SetPosition (0, transform.position);
		line.SetPosition (1, target);
		line.enabled = true;
		Invoke ("laserHide", 0.15f);
	}

	void laserHide(){
		line.enabled = false;
//		attackPermition = false;
	}

	void setAttackRange(SphereCollider arangeC, string tname){
		if (tname == "tank") {
			arangeC.radius = 30.0f;
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
}
