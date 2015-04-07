using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {

	public float speed = 5.0f;
	public int damage = 10;

	Vector3 targetDiretion;

	string targetName;

	bool damageDuple = false;

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
			if(!damageDuple){
				other.gameObject.SendMessage ("applayDamage", damage);
				damageDuple = true;
				Object.Destroy(this.gameObject);
			}
		}

//		if(other.tag == "Player"){
//			other.gameObject.SendMessage ("applayDamage", damage);
//			Object.Destroy(this.gameObject);
//		}
	}

	void dead(){
		Destroy (gameObject);
	}

	void OnDestroy(){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0)};
		mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
		mesh.triangles = new int[] {0, 1, 2};
	}

	public void setDirection(Vector3 thisPosition, Vector3 targetPosition){
		Vector3 result = targetPosition - thisPosition;
		targetDiretion = result.normalized;
	}

	public void setTarget(string targetName){
		this.targetName = targetName;
	}
}
