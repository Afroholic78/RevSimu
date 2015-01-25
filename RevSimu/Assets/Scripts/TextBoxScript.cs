using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.IO;

public class TextBoxScript : MonoBehaviour {
	private JSONLoader loader = null;
	private SceneNode currentNode = null;

	public Text rightNameText; // reference to text component in right name box
	public Text leftNameText; // reference to text component in left name box
	public Text convoText; // reference to text component in dialogue box
	
	public Image rightNameBackground; // reference to background image in right image box
	public Image leftNameBackground; // reference to background image in left image box
	public Image convoBackground; // reference to backgroun image in dialogue box

	public CanvasGroup rightNameCanvasGroup; // reference to right name canvasGroup
	public CanvasGroup leftNameCanvasGroup; // reference to left name canvasGroup
	public CanvasGroup convoCanvasGroup; // reference to dialogue canvasGroup

	private bool fadeOut = false; // Fade out boolean. If false: rightNameBox is visibile, leftNameBox is invisible
	private float duration = 1.0f; // Duration float used for fading
	public float speed = 0f; // Speed of fade

	public void FadeOut() {
		// Function makes the passed GameObject fade out.
		// Will be called during click and timer events

		//Color textureColor = renderer.material.color; // Color object, used for alpha channel
		if (fadeOut == false)
		{ 
			convoCanvasGroup.alpha -= Time.deltaTime/duration;
			if (convoCanvasGroup.alpha == 0) 
			{
				fadeOut = true;
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		GameObject sceneControl = GameObject.Find ("SceneControl");
		SceneController control = sceneControl.GetComponent<MonoBehaviour> () as SceneController;

		loader = new JSONLoader (control.getJSONFilename());
		if (loader == null) {
			Debug.LogError("Loader was null...");
		}

		SceneNode startNode = loader.getSceneNode (control.getStartNode ());
		if (startNode == null) {
			Debug.LogError("startNode is null");
		}
		 
		// Find and associate text box components
		GameObject convoGO = GameObject.Find("ConvoText");
		GameObject convoBackgroundGO = GameObject.Find("ConvoBackground");
		GameObject convoCanvasGO = GameObject.Find("ConvoCanvas");
		convoCanvasGroup = convoCanvasGO.GetComponent <CanvasGroup> (); // find canvas group
		convoBackground = convoBackgroundGO.GetComponent <Image> (); // find image
		convoText = convoGO.GetComponent <Text> (); // find text
		convoText.text = startNode.getMessage (); // set text

		GameObject rightNameGO = GameObject.Find("RightNameText");
		GameObject rightNameBackgroundGO = GameObject.Find("RightNameBackground");
		rightNameBackground = rightNameBackgroundGO.GetComponent <Image> ();
		rightNameText = rightNameGO.GetComponent <Text> ();
		// TODO add right character's name to JSON

		GameObject leftNameGO = GameObject.Find("LeftNameText");
		GameObject LeftNameBackgroundGO = GameObject.Find("LeftNameBackground");
		leftNameBackground = LeftNameBackgroundGO.GetComponent <Image> ();
		leftNameText = leftNameGO.GetComponent <Text> ();
		leftNameText.text = startNode.getCharName ();			
	}
	
	// Update is called once per frame
	void Update () {
		// Test fade out
		//Invoke ("FadeOut(convoBackground)", 2);
		FadeOut();
	}
}
