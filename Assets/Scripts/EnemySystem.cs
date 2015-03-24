using UnityEngine;
using System.Collections;

public class EnemySystem : MonoBehaviour {

	UnitControl[] uc;
	EnemyControl[] ec;

	UnitSystem us;

	// Use this for initialization
	void Start () {
		us = UnitSystem.instance;
	}
	
	// Update is called once per frame
	void Update () {
		uc = us.getUnits ();
		ec = us.getEnemy ();
	}

	void calDistanceAll(EnemyControl en, UnitControl[] units){
		foreach(UnitControl uc in units){
			calDistance(en, uc);
		}
	}

	float calDistance(EnemyControl en, UnitControl uin){
		return Vector3.Distance (en.transform.position, uin.transform.position);
	}

}
