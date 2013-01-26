using UnityEngine;
using System.Collections;

// from https://github.com/shiffman/The-Nature-of-Code-Examples/blob/master/Processing/chp6_agents/NOC_6_09_Flocking/Boid.pde
public class Boid{
	public Vector3 position = Vector3.zero;
	public Vector3 velocity = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;
	public float desiredSeperation = 1.0f;
	public float maxSpeed = 0.5f;
	public float maxForce = 0.09f;
	public Boid(Vector3 initPosition){
		position = initPosition;
	}
	
	public void run(Boid[] boids, Vector3 target){
		Vector3 seeking = seek(target);
		Vector3 sep = separate(boids);
		// weight forces
		seeking *= 1f;
		sep *= 1.2f;
		acceleration += sep;
		acceleration += seeking;
		velocity += acceleration;
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
		position += velocity;
		acceleration *= 0;
	}
	Vector3 separate (Boid[] boids) {
		Vector3 steering = Vector3.zero;
    	int count = 0;
    	// For every boid in the system, check if it's too close
		for(int i=0;i<boids.Length;i++){
			Boid other = boids[i];
			if(other == this){
				continue;
			}
			float d = 0;
			d = (other.position-position).sqrMagnitude;
			if(d != 0 && d < desiredSeperation){
				steering += ((position-other.position).normalized)/d;
				count++;
			}
		}
		if(count > 0){
			//Debug.Log(count);
			steering /= (float)count;		
		}
		if(steering.magnitude > 0){
			steering = steering.normalized*maxForce;
			steering -= velocity;
			steering = Vector3.ClampMagnitude(steering, maxForce);
		}
		return steering;
	}
	Vector3 seek(Vector3 target) {
    	Vector3 desired = (target-position).normalized;
    	desired *= maxSpeed;
    	// Steering = Desired minus Velocity
    	Vector3 steer = desired-velocity;
    	steer = Vector3.ClampMagnitude(steer, maxForce);  // Limit to maximum steering force
    	return steer;
  	}

}

[RequireComponent(typeof(ParticleSystem))]

public class SwarmScript : MonoBehaviour {

	// Use this for initialization
	Boid[] bees;
	public int amountOfBees = 100;
	public float pulseAmp, pulseRate, pulseMin, pulseMax;
	
	public GameObject target;
	private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[100];

	void Start () {
		bees = new Boid[amountOfBees];
		for (int i = 0; i < bees.Length; i++) {
			bees[i] = new Boid(transform.position + new Vector3(Random.value, Random.value, Random.value));
		}
		GetComponent<ParticleSystem>().Emit(amountOfBees);
	}
	
	// Update is called once per frame
	void Update () {
		
		int plength = particleSystem.GetParticles(particles);
		if(plength == 0){
			GetComponent<ParticleSystem>().Emit(amountOfBees);
		}
		plength = particleSystem.GetParticles(particles);
		for(int i=0; i<plength; i++) {
			bees[i].desiredSeperation = ((Mathf.Cos(Time.time*pulseRate)*Mathf.Sin(Time.time*pulseRate)+1)*0.5f)*(pulseMax-pulseMin)+pulseMin;
			bees[i].run(bees, target.transform.position);
			//particles[i].lifetime = 100;
			particles[i].position = bees[i].position;
			//Debug.DrawLine(bees[i].position, bees[i].position+Vector3.up);
			particles[i].velocity = Vector3.zero;
			//particles[i].size = 1.0f;
		}			  
		particleSystem.SetParticles(particles, plength);
	}
}
