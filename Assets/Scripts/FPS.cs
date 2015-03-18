using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {

	public float updateInterval = 0.5f;

	private float accum = 0;
	private int frames = 0;
	private float timeleft;

	// Use this for initialization
	void Start () {
		timeleft = updateInterval;
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeleft <= 0.0) {
			float fps = accum / frames;
			string format = System.String.Format("{0:F2} FPS", fps);
			guiText.text = format;

			if(fps < 30){
				guiText.material.color = Color.yellow;
			} else{
				if(fps < 10){
					guiText.material.color = Color.red;
				} else {
					guiText.material.color = Color.green;
				}
			}

			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}
}
