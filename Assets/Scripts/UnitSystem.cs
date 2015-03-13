using UnityEngine;
using System.Collections;

namespace UnityEngine.UI.Extensions {

	public class UnitSystem : MonoBehaviour {

		public Camera camera;
		public GameObject background;

//		SelectionBox sb;
		UnitControl[] ob;

		// Use this for initialization
		void Start () {
//			sb = SelectionBox.instance;
			background.renderer.material.color = new Color (0, 0, 0, 0.5f);

		}

		void fObject(){
			ob = FindObjectsOfType<UnitControl> ();
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
//						exs.movement();

					}
				}

			}
		}
	}
}