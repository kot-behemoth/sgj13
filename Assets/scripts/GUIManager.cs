using UnityEngine;
using System;

public class GUIManager : MonoBehaviour {

	public GUIText[] playerScores;

	// Allows to have static methods with dynamic variables
	private static GUIManager instance;

	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
	}

	void Update () {

	}

	private void GameStart() {
	}

	private void GameOver() {
		Debug.Log("GUIManager GAME OVER");
	}

	public static void SetPlayerScore(int playerNumber, int playerScore) {
		instance.playerScores[playerNumber-1].text = String.Format("Player {0}: {1}", playerNumber, playerScore);
	}

}
