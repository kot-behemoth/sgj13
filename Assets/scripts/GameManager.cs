using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public ArrayList players;

	public static GameManager instance;

	void Start () {
		instance = this;
		instance.players = new ArrayList();
	}

	void Update () {

	}

	public static void RegisterPlayer(GameObject player) {
		instance.players.Add(player);
	}

}
