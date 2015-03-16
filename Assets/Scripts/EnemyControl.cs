using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public int Hp, maxHp = 100;
	public string typeName;

	// Use this for initialization
	void Start () {
		Hp = maxHp;
	}
	
	// Update is called once per frame
	void Update () {
		unitDead ();
	}

	public void applayDamage(int damage){
		Hp -= damage;
	}

	void unitDead(){
		if (Hp == 0) {
			Object.Destroy(this.gameObject);
		}
	}
}
