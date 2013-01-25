using UnityEngine;
using System.Collections;

public class ExpandTimer : MonoBehaviour {
	AudioManager am;
	public float pulseTime = 1.0f;
	float nextPulse;
	// Use this for initialization
	void Start () {
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		nextPulse = pulseTime;
	}
	
	// Update is called once per frame
	void Update () {
		nextPulse -= Time.deltaTime;
		if(nextPulse<0){
			nextPulse = pulseTime;
			am.expand();
		}
	}
}
