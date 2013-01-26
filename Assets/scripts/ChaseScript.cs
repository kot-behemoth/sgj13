using UnityEngine;
using System.Collections;

// move this object towards a target at defined speed

public class ChaseScript : MonoBehaviour {
	public GameObject target;
	public float speed;
	public GameObject[] players;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject closestPlayer = players[0];
		float distance = Vector3.Distance(players[0].transform.position, transform.position);
		for(int i=1;i<players.Length;i++){
			if(Vector3.Distance(players[i].transform.position, transform.position) < distance){
				distance = Vector3.Distance(players[i].transform.position, transform.position);
				closestPlayer = players[i];
			}
		}
		// if towards
		transform.position += (closestPlayer.transform.position-transform.position).normalized*speed;
		// if away
		//transform.position -= (closestPlayer.transform.position-transform.position).normalized*speed;
	}
}
