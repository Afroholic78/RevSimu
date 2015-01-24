using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public void startGame() {
		Debug.Log ("Start game!");
		Application.LoadLevel ("scene1");
	}

	public void exitGame() {
		Debug.Log ("Exit game!");
		Application.Quit ();
	}
}
