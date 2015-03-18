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

	public float moveSpeed = 1.0f, attackSpeed = 5.0f;
	public int Hp = 0, maxHp = 100;
	public string typeName;
	Vector3 targetPoint, startPoint;
	bool moveStop, attackPermission = true;
//	float distance;
	float distCovered;
	int segments;
	float radius;

//	private ViewRange vr;

	GUIStyle backColor;

	private GameObject bullet1;
	private GameObject damageText;

	LineRenderer line;

	void Start () {

		moveStop = false;
		targetPoint = transform.position;

//		vr = (ViewRange)transform.FindChild ("viewRange").GetComponent (typeof(ViewRange));
		bullet1 = Resources.Load("projectile_001", typeof(GameObject)) as GameObject;
		damageText = Resources.Load ("DamageNumber", typeof(GameObject)) as GameObject;
		line = gameObject.AddComponent<LineRenderer> ();

//		settingCircle ();
//		createPoints ();

		if(Hp == 0)
			Hp = maxHp;
	}

	void FixedUpdate() {

		if (selected) {
			line.enabled = true;
		} else {
			line.enabled = false;
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

		uintDead ();
	}
	
	void OnCollisionEnter(Collision collision){
		moveStop = false;
	}

	public void wayPointSet(Vector3 pos){
		targetPoint = pos;
		targetPoint.z = 0;
		startPoint = transform.position;
	}

	public void attackRotation(Vector3 target){
		if(attackPermission){
			attackPermission = false;
			attack1 (target);
			Invoke("attackPermissionTrue", attackSpeed);
		}

	}

	public void applayDamage(int damage){
		Hp -= damage;
		damageTextShow (damage);
	}

	public Vector3 getPosition(){
		return transform.position;
	}

	public string getName(){
		return typeName;
	}

	public int getCurrentHP(){
		return Hp;
	}

	public int getMaxHp(){
		return maxHp;
	}

	public LineRenderer getLine(){
		return line;
	}

	void uintDead(){
		if (Hp == 0) {
			Object.Destroy(this.gameObject);
		}
	}

	void attackPermissionTrue(){
		attackPermission = true;
	}

	void attack1(Vector3 target){
		GameObject abc = Instantiate (bullet1, transform.position, Quaternion.identity) as GameObject;
		BulletControl bc1 = abc.GetComponent (typeof(BulletControl)) as BulletControl;
		bc1.setDirection (transform.position, target);
		bc1.setTarget ("Enemy");
	}

	void damageTextShow(int damage){
		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		GameObject text = Instantiate (damageText, pos, Quaternion.identity) as GameObject;
		text.SendMessage ("setStartNumber", "-"+damage);
	}
}
