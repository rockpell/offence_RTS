using UnityEngine;
using System.Collections;

public class UnitSystem : MonoBehaviour {

	private static UnitSystem _instance;
	public static UnitSystem instance{
		get{
			if(!_instance){
				_instance = GameObject.FindObjectOfType(typeof(UnitSystem)) as UnitSystem;

				if(!_instance){
					GameObject container = new GameObject();
					container.name = "UnitSystemContainer";
					_instance = container.AddComponent(typeof(UnitSystem)) as UnitSystem;
				}
			}
			return _instance;
		}
	}


	public Camera camera;
	public GameObject background;

	GUIStyle backColor, ShiledBackColor;

//		SelectionBox sb;
	UnitControl[] ob;
	EnemyControl[] ec;

	int segments;
	float radius;

	// Use this for initialization
	void Start () {
//			sb = SelectionBox.instance;
		background.renderer.material.color = new Color (0, 0, 0, 0.5f);

	}

	void fObject(){
		ob = FindObjectsOfType<UnitControl> ();
		ec = FindObjectsOfType<EnemyControl> ();
	}

	// Update is called once per frame
	void Update() {

//		Vector3 pos = Input.mousePosition;
//		pos.z = 1.0f;
		
//			Vector3 stwp = camera.ScreenToWorldPoint(pos);

		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;


		fObject ();

		if (Input.GetMouseButtonDown (1)) {
			foreach(UnitControl exs in ob){
				if(exs.selected){
					if(Physics.Raycast(ray, out hit)){
						exs.wayPointSet(hit.point);
					}
				}
			}

		}

	}

	void OnGUI(){
		int nameGap, hpGap, shieldGap;
		int nameWidth, nameHeight, HpWidth, HpHeight;
		int HpBarWidth = 70;
		
		GUIStyle allingCenter = GUI.skin.GetStyle ("Label");
		allingCenter.alignment = TextAnchor.UpperCenter;

		foreach (UnitControl ogui in ob) {
			string typeName = ogui.getName();
			float Hp = ogui.getCurrentHP();
			float maxHp = ogui.getMaxHp();
			float speedTime = ogui.getAttackSpeed();
			float remainTime = ogui.getRemainAttackTime();
			float Shield = ogui.getCurrentShiled();
			float maxShield = ogui.getMaxShield();

			Vector3 target = ogui.getPosition();
			target = Camera.main.WorldToScreenPoint (target);
			target.y = Screen.height - target.y;
			
			if (typeName == "cube") {
				nameGap = 66;
				hpGap = 52;
				shieldGap = 32;
				nameWidth = 50;
				nameHeight = 20;
				HpWidth = 70;
				HpHeight = 20;
			} else if (typeName == "cylinder") {
				nameGap = 79;
				hpGap = 65;
				shieldGap = 45;
				nameWidth = 50;
				nameHeight = 20;
				HpWidth = 70;
				HpHeight = 20;
			} else if (typeName == "sphere") {
				nameGap = 65;
				hpGap = 51;
				shieldGap = 31;
				nameWidth = 50;
				nameHeight = 20;
				HpWidth = 70;
				HpHeight = 20;
			} else {
				nameGap = 102;
				hpGap = 88;
				shieldGap = 68;
				nameWidth = 50;
				nameHeight = 20;
				HpWidth = 70;
				HpHeight = 20;
				
			}
			
			GUI.Label (new Rect(target.x - nameWidth/2, target.y - nameGap, nameWidth, nameHeight), typeName, allingCenter);
			GUI.Label (new Rect(target.x - HpWidth/2, target.y - hpGap, HpWidth, HpHeight), Hp+" / "+maxHp, allingCenter);
			GUI.Label (new Rect(target.x - HpWidth/2, target.y - shieldGap, HpWidth, HpHeight), Shield + " / " + maxShield, allingCenter);

			
			barBox ();
			
			GUI.Box (new Rect(target.x - HpWidth/2, target.y - hpGap + 5, HpBarWidth * (Hp / maxHp), 14), "", backColor);
			GUI.Box (new Rect(target.x - HpWidth/2, target.y - shieldGap + 3, HpBarWidth * (Shield / maxShield), 14), "", ShiledBackColor);

			if(remainTime != 0)
				GUI.Box(new Rect(target.x - 35, target.y + 25, 5 + 50 * (remainTime / speedTime), 8), "", backColor);

			settingCircle(ogui.getName());
			createPoints(ogui.getLine());
		}

		if (ec != null) {
			foreach (EnemyControl ecui in ec) {
				string typeName = ecui.getName ();
				float Hp = ecui.getCurrentHP ();
				float maxHp = ecui.getMaxHp ();
				float speedTime = ecui.getAttackSpeed();
				float remainTime = ecui.getRemainAttackTime();
				float Shield = ecui.getCurrentShield();
				float maxShield = ecui.getMaxShield();
				
				Vector3 target = ecui.getPosition ();
				target = Camera.main.WorldToScreenPoint (target);
				target.y = Screen.height - target.y;

				if (typeName == "enemy_001") {
					nameGap = 66;
					hpGap = 52;
					shieldGap = 32;
					nameWidth = 68;
					nameHeight = 20;
					HpWidth = 70;
					HpHeight = 20;
				} else {
					nameGap = 102;
					hpGap = 88;
					shieldGap = 68;
					nameWidth = 50;
					nameHeight = 20;
					HpWidth = 70;
					HpHeight = 20;
				}

				GUI.Label (new Rect (target.x - nameWidth / 2, target.y - nameGap, nameWidth, nameHeight), typeName, allingCenter);
				GUI.Label (new Rect (target.x - HpWidth / 2, target.y - hpGap, HpWidth, HpHeight), Hp + " / " + maxHp, allingCenter);
				GUI.Label(new Rect(target.x - HpWidth / 2, target.y - shieldGap, HpWidth, HpHeight), Shield + " / " + maxShield, allingCenter);
				
				GUI.Box (new Rect (target.x - HpWidth / 2, target.y - hpGap + 5, HpBarWidth * (Hp / maxHp), 14), "", backColor);
				GUI.Box (new Rect(target.x - HpWidth/2, target.y - shieldGap + 3, HpBarWidth * (Shield / maxShield), 14), "", ShiledBackColor);

				if(remainTime != 0)
					GUI.Box(new Rect(target.x - 35, target.y + 25, 5 + 50 * (remainTime / speedTime), 8), "", backColor);
			}
		}

	}
	
	void barBox(){
		if (backColor == null) {
			backColor = new GUIStyle(GUI.skin.box);
			backColor.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
		}

		if (ShiledBackColor == null) {
			ShiledBackColor = new GUIStyle(GUI.skin.box);
			ShiledBackColor.normal.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.5f));
		}
	}
	
	private Texture2D MakeTex( int width, int height, Color col ){
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}

	void createPoints(LineRenderer line){
		float x, y = 0f, z = 0f;
		float angle = 20f;
		
		line.SetVertexCount (segments + 1);
		line.useWorldSpace = false;
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.SetColors (Color.green, Color.green);
		
		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
			z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
			
			line.SetPosition(i, new Vector3(x, y, z));
			
			angle += (360f / segments);
		}
	}
	
	void settingCircle(string typeName){
		if (typeName == "cube") {
			segments = 30;
			radius = 1.0f;
		} else if (typeName == "cylinder") {
			segments = 35;
			radius = 1.1f;
		} else if (typeName == "sphere") {
			segments = 30;
			radius = 0.6f;
		} else if(typeName == "tank"){
			segments = 40;
			radius = 2.0f;
		} else {
			segments = 40;
			radius = 2.0f;
		}
	}

	public UnitControl[] getUnits(){
		return ob;
	}

	public EnemyControl[] getEnemy(){
		return ec;
	}
}