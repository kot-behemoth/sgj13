using UnityEngine;

public static class GameEventManager {

	public delegate void GameEvent();

	public static event GameEvent GameStart, GameOver;

	// Allows to have static methods with dynamic variables
	private static GUIManager instance;

	public static void TriggerGameStart() {
		if(GameStart != null)
			GameStart();
	}

	public static void TriggerGameOver() {
		if(GameOver != null)
			GameOver();
	}
}
