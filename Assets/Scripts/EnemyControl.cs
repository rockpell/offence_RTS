using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public int Hp, maxHp = 100, Shield, maxShield = 100;
	public string typeName;
	public float moveSpeed = 1.0f, attackSpeed = 5.0f, hittingR = 10.0f;

	public Transform probePoint; // forward probe point
	public Transform leftR; // left probe point
	public Transform rightR; // right probe point
	public Transform Arm1;

	public Mesh meshToCollide1, meshToCollide2, meshToCollide3;

	bool attackPermission = true;
	bool moveStop;
	bool obstacleAvoid  = false;

	float remainTime = 0f, distCovered, callTemp = 0f;
	float probeRange = 60.0f;

	LineRenderer line;

	private GameObject bullet1;
	private GameObject damageText;

	private Transform obstacleInPath;

	private Vector3 targetPoint;

	NavMeshAgent agent;

	UnitControl[] units;

	UnitSystem us;

	// Use this for initialization
	void Start () {

		moveStop = false;
		targetPoint = transform.position;

		line = gameObject.AddComponent<LineRenderer> ();
		bullet1 = Resources.Load("projectile_001", typeof(GameObject)) as GameObject;
		damageText = Resources.Load ("DamageNumber", typeof(GameObject)) as GameObject;
		agent = GetComponent<NavMeshAgent> ();

		unitSetting ();

		if(probePoint == null)
			probePoint = transform;
		if(leftR == null)
			leftR = transform;         
		if(rightR == null)
			rightR = transform;

		if(Hp == 0)
			Hp = maxHp;
		if (Shield == 0)
			Shield = maxShield;

		us = UnitSystem.instance;

		if(meshToCollide1 != null)
			settingCollider ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		unitDead ();

		RaycastHit hit;
		RaycastHit[] hits;
		Vector3 dir = (targetPoint - transform.position).normalized;
		bool previousCastMissed = true;
		
		Debug.DrawRay(probePoint.position, transform.forward*100, Color.red, 0.2f, true);
		hits = Physics.RaycastAll (probePoint.position, transform.forward, probeRange);
		
		foreach(var hi in hits){
			//			Debug.Log(typeName+" <- my name   found : "+ hi.transform.name);
			if(hi.transform.tag == "Object"){
				if(obstacleInPath == null || obstacleInPath.position != targetPoint){
					Debug.DrawLine(transform.position, hi.point, Color.green);
					previousCastMissed = false;
					obstacleAvoid = true;
					agent.Stop(true);
					agent.ResetPath();
					if(hi.transform != transform){
						obstacleInPath = hi.transform;
						dir += hi.normal * agent.angularSpeed;
					}
				}
			}
		}
		
		//		if(Physics.Raycast(probePoint.position, transform.forward, out hit, probeRange)){
		//			Debug.Log("found : "+ hit.transform.name);
		//			if(obstacleInPath == null || obstacleInPath.position != targetPoint){
		//				Debug.DrawLine(transform.position, hit.point, Color.green);
		//				previousCastMissed = false;
		//				obstacleAvoid = true;
		//				agent.Stop(true);
		//				agent.ResetPath();
		//				if(hit.transform != transform){
		//					obstacleInPath = hit.transform;
		//					dir += hit.normal * agent.angularSpeed;
		//				}
		//			}
		//		}
		
		if(obstacleAvoid  && previousCastMissed && Physics.Raycast(leftR.position, transform.forward, out hit, probeRange)) {
			if(obstacleInPath == null || obstacleInPath.position != targetPoint) { // ignore our target
				Debug.DrawLine(leftR.position, hit.point, Color.red);
				obstacleAvoid = true;
				agent.Stop();
				if(hit.transform != transform) {
					obstacleInPath = hit.transform;
					previousCastMissed = false;
//					Debug.Log("moving around an object");
					dir += hit.normal * agent.angularSpeed;
				}
			}
		}
		
		if(obstacleAvoid && previousCastMissed && Physics.Raycast(rightR.position, transform.forward, out hit, probeRange)) {
			if(obstacleInPath.position != targetPoint) { // ignore our target
				Debug.DrawLine(rightR.position, hit.point, Color.green);
				obstacleAvoid = true;
				agent.Stop();
				if(hit.transform != transform) {
					obstacleInPath = hit.transform;
//					Debug.Log("moving around an object2");
					dir += hit.normal * agent.angularSpeed;
				}
			}
		}
		
		if(obstacleInPath != null){
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			Vector3 toOther = obstacleInPath.position - transform.position;
			if(Vector3.Dot(forward, toOther) < 0){
				obstacleAvoid = false;
				obstacleInPath = null;
				agent.ResetPath();
				agent.SetDestination(targetPoint);
				agent.Resume();
			}
		}
		
		if(obstacleAvoid){
			Quaternion rot = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
			transform.position += transform.forward * agent.speed * Time.deltaTime;
		}

		if (remainTime > 0) {
			remainTime -= Time.deltaTime;
		} else if(remainTime <= 0){
			attackPermissionTrue();
		}

		callTemp += Time.deltaTime;

		if (callTemp > 1.0f) {
			callTemp = 0f;
			units = us.getUnits();
			targetPoint = calDistanceAll(units);
//			Debug.Log(typeName+"  "+Vector3.Distance(targetPoint, transform.position));
			if(targetPoint.y != -9999){
				wayPointSet(targetPoint);
				if(Vector3.Distance(targetPoint, transform.position) < 80){
					agent.Stop();
				}
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
		targetPoint.y = 0;
		agent.SetDestination (targetPoint);
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
//		Debug.Log ("enemy damaged");
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
		GameObject abc = Instantiate (bullet1, Arm1.position, Quaternion.identity) as GameObject;
		BulletControl bc1 = abc.GetComponent (typeof(BulletControl)) as BulletControl;
		bc1.setDirection (Arm1.position, hittingRatio(target));
		bc1.setTarget ("Player");
	}

	void damageTextShow(int damage){
		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		GameObject text = Instantiate (damageText, pos, Quaternion.identity) as GameObject;
		text.SendMessage ("setStartNumber", "-"+damage);
	}

	void unitSetting(){
		if (typeName == "enemy_1") {
			maxHp = 300;
			maxShield = 350;
		} else if (typeName == "enemy_2") {
			maxHp = 100;
			maxShield = 150;
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

	void settingCollider(){
		MeshCollider mc1 = transform.gameObject.AddComponent<MeshCollider>();

		mc1.sharedMesh = meshToCollide1;

		if (meshToCollide2 != null) {
			MeshCollider mc2 = transform.gameObject.AddComponent<MeshCollider>();
			
			mc2.sharedMesh = meshToCollide2;
		}

		if (meshToCollide3 != null) {
			MeshCollider mc3 = transform.gameObject.AddComponent<MeshCollider>();
			
			mc3.sharedMesh = meshToCollide3;
		}
	}
}