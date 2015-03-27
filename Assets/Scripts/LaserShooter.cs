using UnityEngine;
using System.Collections;

public class LaserShooter : MonoBehaviour {

	LineRenderer line;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
		line = gameObject.AddComponent<LineRenderer> ();

		line.SetWidth (1, 1);
		line.SetColors (Color.red, Color.red);
		line.SetVertexCount (2);
		line.useWorldSpace = true;
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void laserCall(Vector3 target){
		Vector3 temp = transform.TransformDirection (target);
		
		if(Physics.Raycast(transform.position, temp, out hit, 200.0f)){
			Debug.Log( target + " <- target position  hit position- >  " + hit.transform.position);
		}
		Debug.Log ("laserDraw : " + target);

		laserDraw (target);
	}

	void laserDraw(Vector3 target){
		line.SetPosition (0, transform.position);
		line.SetPosition (1, target);
		line.enabled = true;
		Invoke ("laserHide", 0.15f);
	}

	void laserHide(){
		line.enabled = false;
	}
}
