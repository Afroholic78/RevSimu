using UnityEngine;
using System.Collections;

public class TitleFader : MonoBehaviour {

	private bool startScene = true;

	void FadeIn() {
		// Fade in scene

		GameObject titelGO = GameObject.Find("GameTitle");
		Color oldC = titelGO.GetComponent<SpriteRenderer> ().color;
		
		if (startScene)
		{   
			float b = Mathf.PingPong(Time.time, 1) / 1;
			Color c = new Color(oldC.r, oldC.g, oldC.b, b);
			titelGO.GetComponent<SpriteRenderer> ().color = c;
			if (b >= 0.99f)
			{
				startScene = false;
			}
		}
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		FadeIn();
	}
}
