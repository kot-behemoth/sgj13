using UnityEngine;
using System.Collections;

// move this object towards a target at defined speed

public class ChaseScript : MonoBehaviour {

	enum States : int { Seeking, RunningAway, Stunned, Attacking };

	private States currentState = States.Seeking;

	public GameObject bear;
	private Animator animator;

	public GameObject target;
	public float speed;
	public GameObject[] players;

	public float stunTime = 2f;
	private float stunnedTime;

	public float runawayTime = 2f;
	public float runawayedTime;

	void Start () {
		//players = GameObject.Find("GameManager").GetComponent<GameManager>().players;
		animator = bear.GetComponent<Animator>();
	}

	void Update () {

		switch((int)currentState) {
			case (int)States.Seeking:
				Seeking();
				break;

			case (int)States.RunningAway:
				runawayedTime -= Time.deltaTime;
				RunningAway();
				if(runawayedTime <= 0) {
					animator.SetBool("isRunaway", false);
					currentState = States.Seeking;
				}
				break;

			case (int)States.Stunned:
				stunnedTime -= Time.deltaTime;
				if(stunnedTime <= 0) {
					animator.SetBool("isStunned", false);
					currentState = States.Seeking;
					rigidbody.isKinematic = false;
				}
				break;

			case (int)States.Attacking:
				break;
		}

	}

	public void GotStunned() {
		animator.SetBool("isStunned", true);
		rigidbody.isKinematic = true;
		currentState = States.Stunned;
		stunnedTime = stunTime;
	}

	public void HitPlayer() {
		animator.SetBool("isRunaway", true);
		currentState = States.RunningAway;
		runawayedTime = runawayTime;
	}

	private void Seeking() {
		players = (GameObject[])GameManager.instance.players.ToArray(typeof(GameObject));
		GameObject closestPlayer = players[0];
		float distance = Vector3.Distance(players[0].transform.position, transform.position);
		for(int i=1;i<players.Length;i++){
			if(Vector3.Distance(players[i].transform.position, transform.position) < distance){
				distance = Vector3.Distance(players[i].transform.position, transform.position);
				closestPlayer = players[i];
			}
		}
		Vector3 vectorToClosestPlayer = closestPlayer.transform.position-transform.position;
		animator.SetFloat("distanceFromPlayer", vectorToClosestPlayer.magnitude);
		transform.position += vectorToClosestPlayer.normalized*speed;
		transform.LookAt(closestPlayer.transform);
	}

	private void RunningAway() {
		players = (GameObject[])GameManager.instance.players.ToArray(typeof(GameObject));
		GameObject closestPlayer = players[0];
		float distance = Vector3.Distance(players[0].transform.position, transform.position);
		for(int i=1;i<players.Length;i++){
			if(Vector3.Distance(players[i].transform.position, transform.position) < distance){
				distance = Vector3.Distance(players[i].transform.position, transform.position);
				closestPlayer = players[i];
			}
		}
		Vector3 vectorToClosestPlayer = closestPlayer.transform.position-transform.position;
		animator.SetFloat("distanceFromPlayer", vectorToClosestPlayer.magnitude);
		transform.position -= vectorToClosestPlayer.normalized*speed;
		transform.LookAt(closestPlayer.transform);
	}

}
