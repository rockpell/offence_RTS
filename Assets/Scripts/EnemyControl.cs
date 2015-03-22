using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public int Hp, maxHp = 100;
	public string typeName;
	public float moveSpeed = 1.0f, attackSpeed = 5.0f;

	bool attackPermission = true;
	bool moveStop;

	float remainTime = 0f, distCovered;

	LineRenderer line;

	private GameObject bullet1;
	private GameObject damageText;

	private Vector3 targetPoint;

	// Use this for initialization
	void Start () {

		moveStop = false;
		targetPoint = transform.position;

		line = gameObject.AddComponent<LineRenderer> ();
		bullet1 = Resources.Load("projectile_001", typeof(GameObject)) as GameObject;
		damageText = Resources.Load ("DamageNumber", typeof(GameObject)) as GameObject;

		hpSet ();
		if(Hp == 0)
			Hp = maxHp;
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
			distCovered = moveSpeed * Time.deltaTime*10;
			transform.position = Vector3.MoveTowards(transform.position, targetPoint, distCovered);
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
		targetPoint.z = 0;
	}

	public void applayDamage(int damage){
		Hp -= damage;
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
		bc1.setDirection (transform.position, target);
		bc1.setTarget ("Player");
	}

	void damageTextShow(int damage){
		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		GameObject text = Instantiate (damageText, pos, Quaternion.identity) as GameObject;
		text.SendMessage ("setStartNumber", "-"+damage);
	}

	void hpSet(){
		if (typeName == "enemy_001") {
			maxHp = 120;
		} else {
			maxHp = 100;
		}
	}

}