using UnityEngine;
using System.Collections;

public class LaserShooter : MonoBehaviour {

	UnitControl uc;

	LineRenderer line;
	RaycastHit[] hit;

	Vector3 targetPosition;
	Vector3 armPositon;

	int laserDamage = 10;

	bool attackPermition = false;

	private GameObject testObject;

//	SphereCollider arangeC;

	// Use this for initialization
	void Start () {
//		arangeC = gameObject.GetComponent<SphereCollider> ();

		uc = (UnitControl)transform.parent.GetComponent (typeof(UnitControl));

		lineSetting ();

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = targetPosition - transform.position;
		float distance = Vector3.Distance(transform.position, targetPosition);

		bool damageDuple = false;

		temp = Vector3.Normalize (temp);
		hit = Physics.RaycastAll (armPositon, temp, distance);

		if (attackPermition) {

			for(var i = 0; i<hit.Length; i++){
				if (hit[i].collider.tag == "Enemy") {
					if(!damageDuple){
						hit[i].collider.gameObject.SendMessage("applayDamage", laserDamage);
						damageDuple = true;
					}
				}
			}

			laserDraw (targetPosition);
			attackPermition = false;
		}

	}



	public void laserCall(Vector3 start, Vector3 target){
		armPositon = start;
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
		line.SetPosition (0, armPositon);
		line.SetPosition (1, target);
		line.enabled = true;
		Invoke ("laserHide", 0.15f);
	}

	void laserHide(){
		line.enabled = false;
//		attackPermition = false;
	}

//	public SphereCollider getCollider(){
//		return arangeC;
//	}
}
