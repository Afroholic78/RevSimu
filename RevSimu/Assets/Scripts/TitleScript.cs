using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {

	void OnMouseEnter() {
		GameObject playGO = GameObject.Find("PlayButton");
		SpriteRenderer playSprite = playGO.GetComponent<SpriteRenderer> ();
		playSprite.sprite = Resources.Load<Sprite> ("Sprites/play_btn_hover");

		Debug.LogWarning("Mouse Over Heart");
	}

	void OnMouseExit() {
		GameObject playGO = GameObject.Find("PlayButton");
		SpriteRenderer playSprite = playGO.GetComponent<SpriteRenderer> ();
		playSprite.sprite = Resources.Load<Sprite> ("Sprites/play_btn_up");
		
		Debug.LogWarning("Mouse Exit Heart");
	}
	
	void OnMouseDown() {
		GameObject playGO = GameObject.Find("PlayButton");
		SpriteRenderer playSprite = playGO.GetComponent<SpriteRenderer> ();
		playSprite.sprite = Resources.Load<Sprite> ("Sprites/play_btn_click");
		Application.LoadLevel(Application.loadedLevel + 1); // Load next scene
		Debug.LogWarning("Mouse Click Heart");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
