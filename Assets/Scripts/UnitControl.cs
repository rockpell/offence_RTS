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

	LineRenderer line;

	void Start () {

		moveStop = false;
		targetPoint = transform.position;

//		vr = (ViewRange)transform.FindChild ("viewRange").GetComponent (typeof(ViewRange));
		bullet1 = Resources.Load("projectile_001", typeof(GameObject)) as GameObject;
		line = gameObject.AddComponent<LineRenderer> ();

		settingCircle ();
		createPoints ();
		if(Hp == 0)
			Hp = maxHp;
	}

	void Update () {

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

//	void OnGUI(){
//		int nameGap, hpGap;
//		int nameWidth, nameHeight, hpWidth, hpHeight;
//
//		GUIStyle allingCenter = GUI.skin.GetStyle ("Label");
//		allingCenter.alignment = TextAnchor.UpperCenter;
//
//		Vector3 target = transform.position;
//		target = Camera.main.WorldToScreenPoint (target);
//		target.y = Screen.height - target.y;
//
//		if (typeName == "cube") {
//			nameGap = 44;
//			hpGap = 30;
//			nameWidth = 50;
//			nameHeight = 20;
//			hpWidth = 70;
//			hpHeight = 20;
//		} else if (typeName == "cylinder") {
//			nameGap = 54;
//			hpGap = 40;
//			nameWidth = 50;
//			nameHeight = 20;
//			hpWidth = 70;
//			hpHeight = 20;
//		} else if (typeName == "sphere") {
//			nameGap = 44;
//			hpGap = 30;
//			nameWidth = 50;
//			nameHeight = 20;
//			hpWidth = 70;
//			hpHeight = 20;
//		} else {
//			nameGap = 82;
//			hpGap = 68;
//			nameWidth = 50;
//			nameHeight = 20;
//			hpWidth = 70;
//			hpHeight = 20;
//
//		}
//
//		GUI.Label (new Rect(target.x - nameWidth/2, target.y-nameGap, nameWidth, nameHeight), typeName, allingCenter);
//		GUI.Label (new Rect(target.x - hpWidth/2, target.y-hpGap, hpWidth, hpHeight), Hp+" / "+maxHp, allingCenter);
//
//
//		barBox ();
//
//		GUI.Box (new Rect(target.x - hpWidth/2, target.y-hpGap + 5, 70, 16), "", backColor);
//		// hp bar ( gui box ) or ( slider )
//	}
//
//	void barBox(){
//		if (backColor == null) {
//			backColor = new GUIStyle(GUI.skin.box);
//			backColor.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
//		}
//	}
//
//	private Texture2D MakeTex( int width, int height, Color col ){
//		Color[] pix = new Color[width * height];
//		for( int i = 0; i < pix.Length; ++i )
//		{
//			pix[ i ] = col;
//		}
//		Texture2D result = new Texture2D( width, height );
//		result.SetPixels( pix );
//		result.Apply();
//		return result;
//	}

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
		BulletControl ddd = abc.GetComponent (typeof(BulletControl)) as BulletControl;
		ddd.setDirection (transform.position, target);
	}

	void createPoints(){
		float x, y, z = 0f;
		float angle = 20f;

		line.SetVertexCount (segments + 1);
		line.useWorldSpace = false;
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.SetColors (Color.green, Color.green);

		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			
			line.SetPosition(i, new Vector3(x, y, z));
			
			angle += (360f / segments);
		}
	}

	void settingCircle(){
		if (typeName == "cube") {
			segments = 30;
			radius = 1.0f;
		} else if (typeName == "cylinder") {
			segments = 35;
			radius = 1.1f;
		} else if (typeName == "sphere") {
			segments = 30;
			radius = 0.6f;
		} else {
			segments = 40;
			radius = 2.0f;
		}
	}

}
