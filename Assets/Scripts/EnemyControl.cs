using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public int Hp, maxHp = 100;
	public string typeName;
	public float moveSpeed = 1.0f, attackSpeed = 5.0f;

	bool attackPermission = true;

	LineRenderer line;

	private GameObject bullet1;
	private GameObject damageText;

	// Use this for initialization
	void Start () {
		Hp = maxHp;
		line = gameObject.AddComponent<LineRenderer> ();
		bullet1 = Resources.Load("projectile_001", typeof(GameObject)) as GameObject;
		damageText = Resources.Load ("DamageNumber", typeof(GameObject)) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		unitDead ();
	}

	public void attackRotation(Vector3 target){
		if(attackPermission){
			attackPermission = false;
			attack1 (target);
			Invoke("attackPermissionTrue", attackSpeed);
		}
	}

	void attackPermissionTrue(){
		attackPermission = true;
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
}
