using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;

public class UnitControl : MonoBehaviour, IBoxSelectable {

	#region Implemented members of IBoxSelectable
	bool _selected = false;
	public bool selected {
		get {
			return _selected;
		}

		set {
			_selected = value;
		}
	}

	bool _preSelected = false;
	public bool preSelected {
		get {
			return _preSelected;
		}
		
		set {
			_preSelected = value;
		}
	}
	#endregion

	public float moveSpeed = 1.0f;
	Vector3 targetPoint, startPoint;
	bool moveStop, attackPermission = true;
	float distance, distCovered;

//	private ViewRange vr;

	private GameObject bullet1;

	//We want the test object to be either a UI element, a 2D element or a 3D element, so we'll get the appropriate components
	SpriteRenderer spriteRenderer;
	Image image;
	Text text;

	void Start () {
		spriteRenderer = transform.GetComponent<SpriteRenderer>();
		image = transform.GetComponent<Image>();
		text = transform.GetComponent<Text>();
		moveStop = false;
		targetPoint = transform.position;

//		vr = (ViewRange)transform.FindChild ("viewRange").GetComponent (typeof(ViewRange));
		bullet1 = Resources.Load("projectile_001", typeof(GameObject)) as GameObject;
	}

	void Update () {

		//What the game object does with the knowledge that it is selected is entirely up to it.
		//In this case we're just going to change the color.

		//White if deselected.
		Color color = Color.white;

		if (preSelected) {
			//Yellow if preselected
			color = Color.yellow;
		}
		if (selected) {
			//And green if selected.
			color = Color.green;
		}

		//Set the color depending on what the game object has.
		if (spriteRenderer) {
			spriteRenderer.color = color;
		} else if (text) {
			text.color = color;
		} else if (image) {
			image.color = color;
		} else if (renderer) {
			renderer.material.color = color;
		}

		if (Vector3.Distance (transform.position, targetPoint) < 0.1f) {
			moveStop = false;
		} else {
			moveStop = true;
		}

		if(moveStop){
			distCovered = moveSpeed * Time.deltaTime*10;
//			transform.position = Vector3.Lerp(startPoint, targetPoint, distCovered/distance);
			transform.position = Vector3.MoveTowards(transform.position, targetPoint, distCovered);
		}
	}

	void OnCollisionEnter(Collision collision){
		moveStop = false;
	}

	public void movement(){
//		transform.Translate (new Vector3(1,0,0));
		transform.position += transform.up * moveSpeed * Time.deltaTime * 100;
//		Debug.Log (transform.forward);
	}


	public void wayPointSet(Vector3 pos){
		targetPoint = pos;
		targetPoint.z = 0;
		startPoint = transform.position;
		distance = Vector3.Distance (startPoint, targetPoint);
//		Debug.Log ("start : " + startPoint + " target : " + targetPoint);
	}

	public void attackRotation(Vector3 target){
		if(attackPermission){
			attackPermission = false;
			attack1 (target);
		}
	}

	void attack1(Vector3 target){

		GameObject abc = Instantiate (bullet1, transform.position, Quaternion.identity) as GameObject;
		BulletControl ddd = abc.GetComponent (typeof(BulletControl)) as BulletControl;
		ddd.setDirection (transform.position, target);
	}

}
