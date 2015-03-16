using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {

	public int segments;
	public float radius;
	LineRenderer line;

	// Use this for initialization
	void Start () {
		segments = 30; 
		radius = 15f;
		line = gameObject.GetComponent<LineRenderer> ();
		line.SetVertexCount (segments + 1);
		createPoints ();
	}

	void createPoints(){
		float x, y, z = 0f;
		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

			line.SetPosition(i, new Vector3(x, y, z));

			angle += (360f / segments);
		}
	}
}
