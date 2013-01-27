using UnityEngine;
using System;

public class GUIManager : MonoBehaviour {

	public GUIText[] playerScores;
	public GUIText winText, instructionsText;

	// Allows to have static methods with dynamic variables
	private static GUIManager instance;

	private bool isGameOver = false;

	private float savedTimeScale;

	void Start () {
		instance = this;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		instance.winText.enabled = false;
		instance.instructionsText.enabled = false;
	}

	void Update () {
		if(isGameOver && (Input.GetButtonDown("Fire11") || Input.GetButtonDown("Fire12"))) {
			GameEventManager.TriggerGameStart();
			isGameOver = false;
		}
	}

	private void GameStart() {
		instance.winText.enabled = false;
		instance.instructionsText.enabled = false;

		Time.timeScale = instance.savedTimeScale;

		Application.LoadLevel(Application.loadedLevel);
	}

	private void GameOver() {
		instance.winText.enabled = true;
		instance.instructionsText.enabled = true;

		instance.savedTimeScale = Time.timeScale;
		Time.timeScale = 0;

		instance.isGameOver = true;
	}

	public static void SetPlayerScore(int playerNumber, int playerScore) {
		instance.playerScores[playerNumber-1].text = String.Format("Player {0}: {1}", playerNumber, playerScore);
	}

	public static void SetWinningPlayer(int playerNumber) {
		instance.winText.text = String.Format("Player {0} wins!", playerNumber);
	}

}
