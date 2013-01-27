using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	void Start () {
		Invoke("showTitle", 5);
	}

	void Update () {
	}

	private void nextScene () {
		Application.LoadLevel("Main_g");
	}

	private void showTitle () {
		Application.LoadLevel("Main_g");
	}

}
