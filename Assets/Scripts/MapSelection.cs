using UnityEngine;
using System.Collections;

public class MapSelection : MonoBehaviour {

	public void Selecting(string name){
		if (name == "map1") {
			Application.LoadLevel ("001_main_game");
		} else {
			return;
		}
	}
}
