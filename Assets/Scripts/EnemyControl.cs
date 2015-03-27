using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public int Hp, maxHp = 100, Shield, maxShield = 100;
	public string typeName;
	public float moveSpeed = 1.0f, attackSpeed = 5.0f, hittingR = 10.0f;

	bool attackPermission = true;
	bool moveStop;

	float remainTime = 0f, distCovered, callTemp = 0f;

	LineRenderer line;

	private GameObject bullet1;
	private GameObject damageText;

	public Vector3 targetPoint;

	UnitControl[] units;

	UnitSystem us;

	// Use this for initialization
	void Start () {

		moveStop = false;
		targetPoint = transform.position;

		line = gameObject.AddComponent<LineRenderer> ();
		bullet1 = Resources.Load("projectile_001", typeof(GameObject)) as GameObject;
		damageText = Resources.Load ("DamageNumber", typeof(GameObject)) as GameObject;

		unitSetting ();

		if(Hp == 0)
			Hp = maxHp;
		if (Shield == 0)
			Shield = maxShield;

		us = UnitSystem.instance;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		unitDead ();

		if (remainTime > 0) {
			remainTime -= Time.deltaTime;
		} else if(remainTime <= 0){
			attackPermissionTrue();
		}

		if (Vector3.Distance (transform.position, targetPoint) < 0.1f) {
			moveStop = false;
		} else {
			moveStop = true;
		}
		
		if(moveStop){
			distCovered = moveSpeed * Time.deltaTime * 10;
			transform.position = Vector3.MoveTowards(transform.position, targetPoint, distCovered);
		}

		callTemp += Time.deltaTime;

		if (callTemp > 1.0f) {
			callTemp = 0f;
			units = us.getUnits();
			targetPoint = calDistanceAll(units);
			if(targetPoint.y != -9999){
				wayPointSet(targetPoint);
			}
		}
	}

	public void attackRotation(Vector3 target){
		if(attackPermission){
			attackPermission = false;
			attack1 (target);
//			Invoke("attackPermissionTrue", attackSpeed);
			remainTime = attackSpeed;
		}
	}

	void attackPermissionTrue(){
		attackPermission = true;
		remainTime = 0f;
	}

	public void wayPointSet(Vector3 pos){
		targetPoint = pos;
		targetPoint.y = 50;
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

	public int getCurrentShield(){
		return Shield;
	}

	public int getMaxShield(){
		return maxShield;
	}

	public LineRenderer getLine(){
		return line;
	}

	void unitDead(){
		if (Hp == 0) {
			Object.Destroy(this.gameObject);
		}
	}

	void attack1(Vector3 target){
		GameObject abc = Instantiate (bullet1, transform.position, Quaternion.identity) as GameObject;
		BulletControl bc1 = abc.GetComponent (typeof(BulletControl)) as BulletControl;
		bc1.setDirection (transform.position, hittingRatio(target));
		bc1.setTarget ("Player");
	}

	void damageTextShow(int damage){
		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		GameObject text = Instantiate (damageText, pos, Quaternion.identity) as GameObject;
		text.SendMessage ("setStartNumber", "-"+damage);
	}

	void unitSetting(){
		if (typeName == "enemy_001") {
			maxHp = 120;
			maxShield = 100;
		} else {
			maxHp = 100;
			maxShield = 100;
		}
	}

	Vector3 calDistanceAll(UnitControl[] units){
		float lastDis = 1000f, temp;
		Vector3 targetPosition = new Vector3(0, -9999, 0);

		foreach(UnitControl uc in units){
			temp = calDistance(uc);
			if(temp < lastDis){
				lastDis = temp;
				targetPoint = uc.transform.position;
			}

		}

		return targetPoint;
	}
	
	float calDistance(UnitControl uin){
		return Vector3.Distance (transform.position, uin.transform.position);
	}

	Vector3 hittingRatio(Vector3 point){
		float number1 = Random.Range (-hittingR, hittingR);
		float number2 = Random.Range (-hittingR, hittingR);
		
		Vector3 result = new Vector3 (point.x + number1, point.y, point.z + number2);
		
		return result;
	}

}