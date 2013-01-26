using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class PointInCollider : MonoBehaviour {
	public float sphereSize = 30;
	public GameObject[] excludeAreas;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public Vector3 getRandomPoint(){
		BoxCollider c = GetComponent<BoxCollider>();
		Vector3 randomPoint = new Vector3(Random.Range(0, c.size.x), Random.Range(0, c.size.y), Random.Range(0, c.size.z));
		randomPoint -= c.size*0.5f;
		randomPoint += c.center;
		randomPoint = transform.TransformPoint(randomPoint);
		// collide with world points that we don't want to be inside of
		// the two layers are world and spawnexclusion
		if(Physics.CheckSphere(randomPoint, sphereSize, 1<<8|1<<9)){
			return getRandomPoint();
		}
		return randomPoint;
	}
}
