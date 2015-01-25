using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {

	void OnMouseEnter() {
		GameObject playGO = GameObject.Find("PlayButton");
		SpriteRenderer playSprite = playGO.GetComponent<SpriteRenderer> ();
		Sprite hoverSprite = Resources.Load<Sprite> ("play_btn_hvr");
		playSprite.sprite = hoverSprite;
	}
	
	void OnMouseDown() {
		GameObject playGO = GameObject.Find("PlayButton");
		SpriteRenderer playSprite = playGO.GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//OnMouseEnter();	
	}
}
