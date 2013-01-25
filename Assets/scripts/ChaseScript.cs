using UnityEngine;
using System.Collections;

// move this object towards a target at defined speed

public class ChaseScript : MonoBehaviour {
	public GameObject target;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (target.transform.position-transform.position).normalized*speed;
	}
}
