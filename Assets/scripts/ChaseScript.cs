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

	public float stunCooldown = 2f;
	private float stunnedTime;

	public float runawayCooldown = 2f;
	private float runawayedTime;

	public float attackCooldown = 2f;
	private float attackedTime;

	public float attackDistanceThreshold = 3f;

	void Start () {
		animator = bear.GetComponent<Animator>();
	}

	void Update () {

		switch((int)currentState) {
			case (int)States.Seeking:
				animator.SetBool("isSeeking", true);
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
				}
				break;

			case (int)States.Attacking:
				animator.SetBool("isAttacking", true);
				attackedTime -= Time.deltaTime;
				if(attackedTime <= 0) {
					HitPlayer();
					attackedTime = attackCooldown;
				}
				break;
		}

	}

	public void GotStunned() {
		animator.SetBool("isStunned", true);
		currentState = States.Stunned;
		stunnedTime = stunCooldown;
	}

	public void HitPlayer() {
		GameObject closestPlayer = GetClosestPlayer();
		closestPlayer.GetComponent<PlayerScript>().GotHit();
		animator.SetBool("isRunaway", true);

		currentState = States.RunningAway;
		runawayedTime = runawayCooldown;
	}

	private void Seeking() {
		GameObject closestPlayer = GetClosestPlayer();
		Vector3 vectorToClosestPlayer = closestPlayer.transform.position-transform.position;

		if(vectorToClosestPlayer.magnitude <= attackDistanceThreshold) {
			attackedTime = attackCooldown;
			currentState = States.Attacking;
		}
		transform.position += vectorToClosestPlayer.normalized*speed;
		transform.LookAt(closestPlayer.transform);
	}

	private void Attacking() {
		GameObject closestPlayer = GetClosestPlayer();
		Vector3 vectorToClosestPlayer = closestPlayer.transform.position-transform.position;
		if(vectorToClosestPlayer.magnitude > attackDistanceThreshold) {
			animator.SetBool("isSeeking", true);
			currentState = States.Seeking;
		}
	}

	private void RunningAway() {
		GameObject closestPlayer = GetClosestPlayer();
		Vector3 vectorToClosestPlayer = closestPlayer.transform.position-transform.position;
		transform.position -= vectorToClosestPlayer.normalized*speed;
		transform.LookAt(transform.position-vectorToClosestPlayer);
	}

	private GameObject GetClosestPlayer()
	{
		players = (GameObject[])GameManager.instance.players.ToArray(typeof(GameObject));
		GameObject closestPlayer = players[0];
		float distance = Vector3.Distance(players[0].transform.position, transform.position);
		for(int i=1;i<players.Length;i++){
			if(Vector3.Distance(players[i].transform.position, transform.position) < distance){
				distance = Vector3.Distance(players[i].transform.position, transform.position);
				closestPlayer = players[i];
			}
		}
		return closestPlayer;
	}

}
