using UnityEngine;
using System.Collections;

public class EnemySystem : MonoBehaviour {

	UnitControl[] uc;
	EnemyControl[] ec;

	UnitSystem us;

	// Use this for initialization
	void Start () {
		us = UnitSystem.intstance;
	}
	
	// Update is called once per frame
	void Update () {
		uc = us.getUnits ();
		ec = us.getEnemy ();
	}
}
