using UnityEngine;
using System.Collections;

namespace UnityEngine.UI.Extensions {

	public class UnitSystem : MonoBehaviour {

		public Camera camera;
		public GameObject background;

		GUIStyle backColor;

//		SelectionBox sb;
		UnitControl[] ob;
		EnemyControl[] ec;

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
		void Update () {

			Vector3 pos = Input.mousePosition;
			pos.z = 1.0f;
			
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
			int nameGap, hpGap;
			int nameWidth, nameHeight, HpWidth, HpHeight;
			int HpBarWidth = 70;
			
			GUIStyle allingCenter = GUI.skin.GetStyle ("Label");
			allingCenter.alignment = TextAnchor.UpperCenter;

			foreach (UnitControl ogui in ob) {
				string typeName = ogui.getName();
				int Hp = ogui.getCurrentHP();
				int maxHp = ogui.getMaxHp();

				Vector3 target = ogui.getPosition();
				target = Camera.main.WorldToScreenPoint (target);
				target.y = Screen.height - target.y;
				
				if (typeName == "cube") {
					nameGap = 44;
					hpGap = 30;
					nameWidth = 50;
					nameHeight = 20;
					HpWidth = 70;
					HpHeight = 20;
				} else if (typeName == "cylinder") {
					nameGap = 54;
					hpGap = 40;
					nameWidth = 50;
					nameHeight = 20;
					HpWidth = 70;
					HpHeight = 20;
				} else if (typeName == "sphere") {
					nameGap = 44;
					hpGap = 30;
					nameWidth = 50;
					nameHeight = 20;
					HpWidth = 70;
					HpHeight = 20;
				} else {
					nameGap = 82;
					hpGap = 68;
					nameWidth = 50;
					nameHeight = 20;
					HpWidth = 70;
					HpHeight = 20;
					
				}
				
				GUI.Label (new Rect(target.x - nameWidth/2, target.y-nameGap, nameWidth, nameHeight), typeName, allingCenter);
				GUI.Label (new Rect(target.x - HpWidth/2, target.y-hpGap, HpWidth, HpHeight), Hp+" / "+maxHp, allingCenter);

				
				barBox ();
				
				GUI.Box (new Rect(target.x - HpWidth/2, target.y-hpGap + 5, HpBarWidth, 14), "", backColor);
			}

			if (ec != null) {
				foreach (EnemyControl ecui in ec) {
					string typeName = ecui.getName ();
					float Hp = ecui.getCurrentHP ();
					float maxHp = ecui.getMaxHp ();
					
					Vector3 target = ecui.getPosition ();
					target = Camera.main.WorldToScreenPoint (target);
					target.y = Screen.height - target.y;

					if (typeName == "enemy_001") {
						nameGap = 44;
						hpGap = 30;
						nameWidth = 50;
						nameHeight = 20;
						HpWidth = 70;
						HpHeight = 20;
					} else {
						nameGap = 82;
						hpGap = 68;
						nameWidth = 50;
						nameHeight = 20;
						HpWidth = 70;
						HpHeight = 20;
					}

					GUI.Label (new Rect (target.x - nameWidth / 2, target.y - nameGap, nameWidth, nameHeight), typeName, allingCenter);
					GUI.Label (new Rect (target.x - HpWidth / 2, target.y - hpGap, HpWidth, HpHeight), Hp + " / " + maxHp, allingCenter);
					
					GUI.Box (new Rect (target.x - HpWidth / 2, target.y - hpGap + 5, HpBarWidth * (Hp / maxHp), 14), "", backColor);
				}
			}

		}
		
		void barBox(){
			if (backColor == null) {
				backColor = new GUIStyle(GUI.skin.box);
				backColor.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.5f));
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
	}
}