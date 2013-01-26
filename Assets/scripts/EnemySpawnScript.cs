using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnChunk {
    public float time;
    public GameObject enemyType;
	public int count;
	public bool spawned = false;
}

public class EnemySpawnScript : MonoBehaviour {
	public SpawnChunk[] spawnTimes;
	public GameObject player;
	private float lastTime;
	private PointInCollider[] spawnArea;
	public GameObject[] spawners;
	// Use this for initialization
	void Start () {
		spawnArea = new PointInCollider[spawners.Length];
		for (int i = 0; i < spawners.Length; i++) {
			spawnArea[i] = spawners[i].GetComponent<PointInCollider>();
		}
		for (int i = 0; i < spawnTimes.Length; i++) {
			if(spawnTimes[i].time == 0){
				if(!spawnTimes[i].spawned){
					spawnTimes[i].spawned = true;
					spawn (spawnTimes[i].enemyType, spawnTimes[i].count);
				}
			}
		}
		lastTime = 0;
	}
	void spawn(GameObject g, int count){
		for (int i = 0; i < count; i++) {
			Vector3 startPoint = spawnArea[Random.Range(0, spawnArea.Length)].getRandomPoint();
			GameObject e = (GameObject)Instantiate(g, startPoint, Quaternion.identity);		
			if(e.GetComponent<ChaseScript>()){
				e.GetComponent<ChaseScript>().target = player;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < spawnTimes.Length; i++) {
			if(spawnTimes[i].time > lastTime && spawnTimes[i].time < lastTime + Time.deltaTime ){
				if(!spawnTimes[i].spawned){
					spawnTimes[i].spawned = true;
					spawn (spawnTimes[i].enemyType, spawnTimes[i].count);
				}
			}
		}
		lastTime += Time.deltaTime;
		
	}
}
