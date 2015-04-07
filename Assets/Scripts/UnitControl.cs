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

	public float moveSpeed = 1.0f, attackSpeed = 5.0f, attackSpeed2 = 8.0f, hittingR = 10.0f;
//	float MaxTurnSpeed = 40.0f;
	public int Hp = 0, maxHp = 100, Shield = 0, maxShield = 100;
	public string typeName;
	float probeRange = 60.0f;

	public Transform probePoint; // forward probe point
	public Transform leftR; // left probe point
	public Transform rightR; // right probe point

	public Transform leftArm;
	public Transform rightArm;

	public Mesh meshToCollide1, meshToCollide2, meshToCollide3;

	Vector3 targetPoint;

	bool attackPermission = true, attackPermission2 = true;
	bool waypointBool = false;
	bool obstacleAvoid  = false;

	int segments;

	float distCovered;
	float remainTime = 0f, remainTime2 = 0f;

	string nextAttack;

//	private ViewRange vr;
	private LaserShooter ls;

	GUIStyle backColor;

	private GameObject bullet1;
	private GameObject damageText;
	private GameObject waypointObject, wp;

	private Transform TankBody;
	private Transform obstacleInPath;

	NavMeshAgent agent;

	LineRenderer line;

	void Start () {

		targetPoint = transform.position;

		if (typeName == "tank") {
			bullet1 = Resources.Load ("projectile_001", typeof(GameObject)) as GameObject;
		} else {
			bullet1 = Resources.Load ("projectile_001", typeof(GameObject)) as GameObject;
			ls = (LaserShooter)transform.FindChild ("LaserShooter").GetComponent (typeof(LaserShooter));
		}

		damageText = Resources.Load ("DamageNumber", typeof(GameObject)) as GameObject;
		waypointObject = Resources.Load ("waypointObject", typeof(GameObject)) as GameObject;
		line = gameObject.AddComponent<LineRenderer> ();
		agent = GetComponent<NavMeshAgent> ();

//		settingCircle ();
//		createPoints ();

		wp = Instantiate (waypointObject, targetPoint, Quaternion.identity) as GameObject;

		if(typeName == "tank")
			TankBody = transform.FindChild ("TankMesh");
		if(typeName == "bomber")
			TankBody = transform.FindChild ("BomberMesh");

		if(probePoint == null)
			probePoint = transform;
		if(leftR == null)
			leftR = transform;         
		if(rightR == null)
			rightR = transform;
		
		unitSetting ();
		
		if(Hp == 0)
			Hp = maxHp;
		if (Shield == 0)
			Shield = maxShield;

		settingCollider ();
	}

	void FixedUpdate() {
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
					Debug.Log("moving around an object");
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
					Debug.Log("moving around an object2");
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
				Debug.Log("dot");
			}
		}

		if(obstacleAvoid){
			Quaternion rot = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
			transform.position += transform.forward * agent.speed * Time.deltaTime;
			Debug.Log("rotating");
		}

		if (selected) {
			line.enabled = true;
			wp.SetActive(true);
		} else {
			line.enabled = false;
			wp.SetActive(false);
		}

//		if(moveStop){
//			distCovered = moveSpeed * Time.deltaTime*10;
//			transform.position = Vector3.MoveTowards(transform.position, targetPoint, distCovered);
////			rigidbody.MovePosition(transform.position + np * distCovered);
//		}

		uintDead ();

		if (remainTime > 0) {
			remainTime -= Time.deltaTime;
		} else if(remainTime <= 0){
			attackPermissionTrue();
		}

		if(remainTime2 > 0){
			remainTime2 -= Time.deltaTime;
		} else if(remainTime <= 0){
			attackPermissionTrue2();
		}

//		if(typeName == "tank" && waypointBool)
//			unitRotate (targetPoint);
//		if(typeName == "bomber" && waypointBool)
//			unitRotate (targetPoint);

//		agent.SetDestination (targetPoint);
	}
	
	void OnCollisionEnter(Collision collision){

	}

	void waypointObjectSet(){
		Vector3 temp = targetPoint;
		temp.y = 20;
		wp.transform.position = temp;
	}

	public void wayPointSet(Vector3 pos){
		targetPoint = pos;
		targetPoint.y = 0;
		waypointBool = true;
		agent.SetDestination (targetPoint);
		waypointObjectSet ();
//		agent.destination = targetPoint;

//		np = Vector3.Normalize(targetPoint - transform.position);
//		Debug.Log ("target position : " + targetPoint);
	}

	public void attackRotation(Vector3 target, string type){
		if (type == "view") {
			if (attackPermission) {
				attackPermission = false;
				attack1 (target);
				//			Invoke("attackPermissionTrue", attackSpeed);
				remainTime = attackSpeed;
				//			Debug.Log(Time.time);
			}
		} else {
			if(attackPermission2){
				attackPermission2 = false;
				attackLaser1(target);
				remainTime2 = attackSpeed2;
			}
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

	void attackPermissionTrue2(){
		attackPermission2 = true;
		remainTime2 = 0f;
	}

	Vector3 attackProcedure(){
		Vector3 result = new Vector3(0, 0, 0);

		if(typeName == "tank"){
			if(nextAttack == "left"){
				result = leftArm.position;
				nextAttack = "right";
			} else {
				result = rightArm.position;
				nextAttack = "left";
			}
		} else if(typeName == "bomber"){
			result = leftArm.position;
		}

		return result;
	}

	void attack1(Vector3 target){
		Vector3 shootingPosition = attackProcedure();
		GameObject abc = Instantiate (bullet1, shootingPosition, Quaternion.identity) as GameObject;
		BulletControl bc1 = abc.GetComponent (typeof(BulletControl)) as BulletControl;
		bc1.setDirection (shootingPosition, hittingRatio(target));
		bc1.setTarget ("Enemy");
	}

	void attackLaser1(Vector3 target){
		ls.laserCall (rightArm.position, target);
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
		} else if(typeName == "bomber"){
			maxHp = 220;
			maxShield = 310;
			moveSpeed = 1.5f;
		} else {
			maxHp = 100;
			maxShield = 100;
			moveSpeed = 1.0f;
		}
	}

	Vector3 hittingRatio(Vector3 point){
		float number1 = Random.Range (-hittingR, hittingR);
		float number2 = Random.Range (-hittingR, hittingR);

		Vector3 result = new Vector3 (point.x + number1, point.y, point.z + number2);

		return result;
	}

//	void unitRotate(Vector3 target){
//		Vector3 relativePos = target - transform.position;
//		Quaternion rotation = Quaternion.LookRotation(relativePos);
////		TankBody.rotation = rotation;
//		TankBody.rotation =  Quaternion.RotateTowards(TankBody.rotation, rotation, MaxTurnSpeed * Time.deltaTime);
//	}

	void settingCollider(){
		MeshCollider mc1 = transform.gameObject.AddComponent<MeshCollider>();
		MeshCollider mc2 = transform.gameObject.AddComponent<MeshCollider>();
		MeshCollider mc3 = transform.gameObject.AddComponent<MeshCollider>();

		mc1.sharedMesh = meshToCollide1;
		mc2.sharedMesh = meshToCollide2;
		mc3.sharedMesh = meshToCollide3;
	}
}
