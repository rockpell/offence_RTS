﻿using UnityEngine;
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
	int fontSizeContorl;
	float radius;
	float sizeRate = 1.0f;

	// Use this for initialization
	void Start () {
//			sb = SelectionBox.instance;
		background.renderer.material.color = new Color (0, 0, 0, 0);

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
		RaycastHit[] hit;

		fObject ();

		if (Input.GetMouseButtonDown (1)) {
			foreach(UnitControl exs in ob){
				if(exs.selected){
					hit = Physics.RaycastAll(ray);
					for(var i = 0; i< hit.Length; i++){
						if(hit[i].collider.tag == "BackGround"){
							exs.wayPointSet(hit[i].point);
						}
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
		allingCenter.fontSize = 13 + fontSizeContorl;

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
			
			if (typeName == "tank"){
				nameGap = 82;
				hpGap = 68;
				shieldGap = 48;
				nameWidth = 50;
				nameHeight = 20;
				HpWidth = 70;
				HpHeight = 20;
			} else if(typeName == "bomber"){
				nameGap = 92;
				hpGap = 78;
				shieldGap = 58;
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
			
			GUI.Label (new Rect(target.x - nameWidth/2, target.y - nameGap, nameWidth * sizeRate, nameHeight * sizeRate), typeName, allingCenter);
			GUI.Label (new Rect(target.x - HpWidth/2, target.y - hpGap, HpWidth * sizeRate, HpHeight * sizeRate), Hp+" / "+maxHp, allingCenter);
			GUI.Label (new Rect(target.x - HpWidth/2, target.y - shieldGap, HpWidth * sizeRate, HpHeight * sizeRate), Shield + " / " + maxShield, allingCenter);

			
			barBox ();
			
			GUI.Box (new Rect(target.x - HpWidth/2, target.y - hpGap + 5, HpBarWidth * (Hp / maxHp) * sizeRate, 14 * sizeRate), "", backColor);
			GUI.Box (new Rect(target.x - HpWidth/2, target.y - shieldGap + 3, HpBarWidth * (Shield / maxShield * sizeRate), 14 * sizeRate), "", ShiledBackColor);

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

				if (typeName == "enemy_1") {
					nameGap = 76;
					hpGap = 62;
					shieldGap = 42;
					nameWidth = 72;
					nameHeight = 20;
					HpWidth = 70;
					HpHeight = 20;
				} else if (typeName == "enemy_2") {
					nameGap = 76;
					hpGap = 62;
					shieldGap = 42;
					nameWidth = 72;
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

				GUI.Label (new Rect (target.x - nameWidth / 2, target.y - nameGap, nameWidth * sizeRate, nameHeight * sizeRate), typeName, allingCenter);
				GUI.Label (new Rect (target.x - HpWidth / 2, target.y - hpGap, HpWidth * sizeRate, HpHeight * sizeRate), Hp + " / " + maxHp, allingCenter);
				GUI.Label(new Rect(target.x - HpWidth / 2, target.y - shieldGap, HpWidth * sizeRate, HpHeight * sizeRate), Shield + " / " + maxShield, allingCenter);
				
				GUI.Box (new Rect (target.x - HpWidth / 2, target.y - hpGap + 5, HpBarWidth * (Hp / maxHp) * sizeRate, 14 * sizeRate), "", backColor);
				GUI.Box (new Rect(target.x - HpWidth/2, target.y - shieldGap + 3, HpBarWidth * (Shield / maxShield) * sizeRate, 14 * sizeRate), "", ShiledBackColor);

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
		} else if(typeName == "bomber"){
			segments = 45;
			radius = 4.0f;
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

	public void sizeContorl(string text){
		if (text == "up") {
			if(sizeRate < 1.0f){
				sizeRate += 0.02f;
				Debug.Log("up : "+sizeRate);
			}
			if(fontSizeContorl <= 0){
				if(0.89f < sizeRate  && sizeRate < 0.91f){
					fontSizeContorl += 1;
					Debug.Log("font up");
				}
			}
		} else if(text == "down"){
			if(sizeRate > 0.9f){
				sizeRate -= 0.02f;
				Debug.Log("down : "+sizeRate);
			}
			if(fontSizeContorl >= 0){
				if(0.89f < sizeRate  && sizeRate < 0.91f){
					fontSizeContorl -= 1;
					Debug.Log("font down");
				}
			}
		}
	}
}