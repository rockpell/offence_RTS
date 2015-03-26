﻿using UnityEngine;
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
	public int Hp = 0, maxHp = 100, Shield = 0, maxShield = 100;
	public string typeName;

	Vector3 targetPoint;
	Vector3 np;
	
	bool moveStop, attackPermission = true;

	int segments;

	float distCovered;
	float remainTime = 0f;

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

		unitSetting ();

		if(Hp == 0)
			Hp = maxHp;
		if (Shield == 0)
			Shield = maxShield;
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
			transform.position = Vector3.MoveTowards(transform.position, targetPoint, distCovered);
//			rigidbody.MovePosition(transform.position + np * distCovered);
		}

		uintDead ();

		if (remainTime > 0) {
			remainTime -= Time.deltaTime;
		} else if(remainTime <= 0){
			attackPermissionTrue();
		}
	}
	
	void OnCollisionEnter(Collision collision){
		moveStop = false;
	}

	public void wayPointSet(Vector3 pos){
		targetPoint = pos;
		targetPoint.z = 0;
		np = Vector3.Normalize(targetPoint - transform.position);
//		Debug.Log ("target position : " + targetPoint);
	}

	public void attackRotation(Vector3 target){
		if(attackPermission){
			attackPermission = false;
			attack1 (target);
//			Invoke("attackPermissionTrue", attackSpeed);
			remainTime = attackSpeed;
//			Debug.Log(Time.time);
		}

	}

	public void applayDamage(int damage){
		if (Shield == 0) {
			Hp -= damage;
		} else {
			Shield -= damage;
			if(Shield < 0){
				Hp += Shield;
				Shield = 0;
			}
		}

		damageTextShow (damage);
	}

	public float getAttackSpeed(){
		return attackSpeed;
	}

	public float getRemainAttackTime(){
		return remainTime;
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

	public int getCurrentShiled(){
		return Shield;
	}

	public int getMaxShield(){
		return maxShield;
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
		remainTime = 0f;
	}

	void attack1(Vector3 target){
		GameObject abc = Instantiate (bullet1, transform.position, Quaternion.identity) as GameObject;
		BulletControl bc1 = abc.GetComponent (typeof(BulletControl)) as BulletControl;
		bc1.setDirection (transform.position, hittingRatio(target));
		bc1.setTarget ("Enemy");
	}

	void damageTextShow(int damage){
		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		GameObject text = Instantiate (damageText, pos, Quaternion.identity) as GameObject;
		text.SendMessage ("setStartNumber", "-"+damage);
	}

	void unitSetting(){
		if (typeName == "cube") {
			maxHp = 120;
			maxShield = 100;
			moveSpeed = 1.8f;
		} else if (typeName == "sphere") {
			maxHp = 130;
			maxShield = 100;
			moveSpeed = 1.5f;
		} else if (typeName == "cylinder") {
			maxHp = 150;
			maxShield = 100;
			moveSpeed = 1.2f;
		} else if (typeName == "tank") {
			maxHp = 200;
			maxShield = 300;
			moveSpeed = 0.8f;
		} else {
			maxHp = 100;
			maxShield = 100;
			moveSpeed = 1.0f;
		}
	}

	Vector3 hittingRatio(Vector3 point){
		float number1 = Random.Range (-10f, 10f);
		float number2 = Random.Range (-10f, 10f);

		Vector3 result = new Vector3 (point.x + number1, point.y + number2, 0);

		return result;
	}
}
