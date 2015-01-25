using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class TextBoxScript : MonoBehaviour {
	private JSONLoader loader = null;
	private SceneNode currentNode = null;

	private List<GameObject> buttons;

	private Dictionary<string, Sprite> loadedSprites;
	private string curLeftSpriteName = null;
	private string curRightSpriteName = null;

	public Text rightNameText; // reference to text component in right name box
	public Text leftNameText; // reference to text component in left name box
	public Text convoText; // reference to text component in dialogue box
	
	public Image rightNameBackground; // reference to background image in right image box
	public Image leftNameBackground; // reference to background image in left image box
	public Image convoBackground; // reference to backgroun image in dialogue box

	public CanvasGroup rightNameCanvasGroup; // reference to right name canvasGroup
	public CanvasGroup leftNameCanvasGroup; // reference to left name canvasGroup
	public CanvasGroup convoCanvasGroup; // reference to dialogue canvasGroup
	
	private float duration = 0.15f; // Duration float used for fading
	public float speed = 0f; // Speed of text box fade
	public float fadeSpeed = 1.5f; // Speed of scene fade

    private bool toggle = true;
	private bool startScene = true;
	private bool endScene = false;

	private void FadeOut() {
		if (endScene) return;
		// Function fades out text boxes containing character
		// names deoending on who is speaking.

		// If right side character is speaking, fade out
		// If left side characters is speaking

		float leftAlpha = leftNameCanvasGroup.alpha;
		float rightAlpha = rightNameCanvasGroup.alpha;

		if (currentNode.isRightTalking() == true)
		{
			if (leftNameCanvasGroup.alpha <= 0.05f) { leftNameCanvasGroup.alpha = 0; }
			else if (leftNameCanvasGroup.alpha > 0) { leftNameCanvasGroup.alpha -= Time.deltaTime/duration; } // left fade out 
			if (rightNameCanvasGroup.alpha >= 1f) { rightNameCanvasGroup.alpha = 1; }
			else if (rightNameCanvasGroup.alpha < 1) { rightNameCanvasGroup.alpha += Time.deltaTime/duration; } // right fade i
		}

		if (currentNode.isRightTalking() == false)
		{
			if (leftNameCanvasGroup.alpha >= 1f) { leftNameCanvasGroup.alpha = 1; }
			else if (leftNameCanvasGroup.alpha < 1) { leftNameCanvasGroup.alpha += Time.deltaTime/duration; } // left fade in
			if (rightNameCanvasGroup.alpha <= 0.05f) { rightNameCanvasGroup.alpha = 0; }
			else if (rightNameCanvasGroup.alpha > 0) { rightNameCanvasGroup.alpha -= Time.deltaTime/duration; } // right fade out
		}
	}
	
	// Use this for initialization
	void Start () {
		GameObject sceneControl = GameObject.Find ("SceneControl");
		SceneController control = sceneControl.GetComponent<MonoBehaviour> () as SceneController;

		loader = new JSONLoader (control.getJSONFilename()); // initialize JSON loader
		if (loader == null) {
			Debug.LogError("Loader was null");
		}

		this.currentNode = loader.getSceneNode (control.getStartNode (), this.currentNode); // load scene JSON nodes
		if (currentNode == null) {
			Debug.LogError("startNode is null");
		}
		 
		// Find and associate text box components
		GameObject convoGO = GameObject.Find("ConvoText");
		GameObject convoBackgroundGO = GameObject.Find("ConvoBackground");
		GameObject convoCanvasGO = GameObject.Find("ConvoCanvas");
		convoCanvasGroup = convoCanvasGO.GetComponent <CanvasGroup> (); // find canvas group
		convoBackground = convoBackgroundGO.GetComponent <Image> (); // find image
		convoText = convoGO.GetComponent <Text> (); // find text
		convoText.text = currentNode.getMessage (); // set text

		GameObject rightNameGO = GameObject.Find("RightNameText");
		GameObject rightNameBackgroundGO = GameObject.Find("RightNameBackground");
		GameObject righNameCanvasGO = GameObject.Find("RightNameCanvas");
		rightNameCanvasGroup = righNameCanvasGO.GetComponent <CanvasGroup> (); // find canvas group
		rightNameBackground = rightNameBackgroundGO.GetComponent <Image> (); // find image
		rightNameText = rightNameGO.GetComponent <Text> (); // find text
		rightNameText.text = currentNode.getCharName (); // set text

		GameObject leftNameGO = GameObject.Find("LeftNameText");
		GameObject LeftNameBackgroundGO = GameObject.Find("LeftNameBackground");
		GameObject leftNameCanvasGO = GameObject.Find("LeftNameCanvas");
		leftNameCanvasGroup = leftNameCanvasGO.GetComponent <CanvasGroup> (); // find canvas group
		leftNameBackground = LeftNameBackgroundGO.GetComponent <Image> (); // find image
		leftNameText = leftNameGO.GetComponent <Text> (); // find text
		leftNameText.text = currentNode.getCharName (); // set text

		// Decide which name box to show first
		//if (currentNode.isRightTalking() == true) { leftNameCanvasGroup.alpha = 0; }
		//if (currentNode.isRightTalking() == false) { rightNameCanvasGroup.alpha = 0; }


		buttons = new List<GameObject> ();
		buttons.Add(GameObject.Find("OptionButton0"));
		buttons.Add(GameObject.Find("OptionButton1"));
		buttons.Add(GameObject.Find("OptionButton2"));
		setUpOptions ();
		//buttons[0].AddComponent<SpeechBubbleFloat>();

		setUpSprites ();
	}

	void FadeInScene() {
		// Fade in scene
		if (startScene == true)
		{
			GameObject faderGO = GameObject.Find("Fader");
			SpriteRenderer fader = faderGO.GetComponent<SpriteRenderer> ();
			fader.color = Color.Lerp(fader.color, Color.clear, fadeSpeed * Time.deltaTime);
			if (fader.color.a <= 0.05f) { startScene = false; } // flip boolean once fade in is over
		}
	}

	void FadeOutScene() {
		// Fade out scene
		if (endScene == true)
		{
			GameObject faderGO = GameObject.Find("Fader");
			SpriteRenderer fader = faderGO.GetComponent<SpriteRenderer> ();
			fader.color = Color.Lerp(fader.color, Color.black, fadeSpeed * Time.deltaTime);
			Debug.LogWarning(fader.color.a);
			if (fader.color.a >= .95f) 
			{ 
				endScene = false; // flip boolean once fade out is over
				Application.LoadLevel(Application.loadedLevel + 1); // Load next scene
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		FadeInScene();
		FadeOut(); // Perform fading operations
		FadeOutScene();
	}

    void speedincrease()
    {
        speed += 0.01f;
    }
    void fade()
    {
        int i = 0;
        for (; i < currentNode.getOptionCount(); ++i)
        {
            Color oldC = buttons[i].GetComponent<Image>().color;
            
            if (toggle)
            {   
                float b =  1 - Mathf.PingPong(Time.time, 1) / 1;
                Color c = new Color(oldC.r, oldC.g, oldC.b, b);
                buttons[i].GetComponent<Image>().color = c;
                if (b == 0)
                {
                    toggle = false;
                }
            }
            else
            {
                float b = 1 - Mathf.PingPong(Time.time + speed, 1) / 1;
                Color c = new Color(oldC.r, oldC.g, oldC.b, b);
                buttons[i].GetComponent<Image>().color = c;
                Invoke("speedincrease", 2);
            }
        }
    }
	public void option0() {
		Debug.Log ("option0: " + currentNode.getOption(0));
		currentNode = loader.getSceneNode (currentNode.getOption (0),
		                                   this.currentNode);
		setUpText ();
		setUpOptions ();
		setUpSprites ();
	}
	
	public void option1() {
		Debug.Log ("option1: " + currentNode.getOption(1));
		currentNode = loader.getSceneNode (currentNode.getOption (1),
		                                   this.currentNode);
		setUpText ();
		setUpOptions ();
		setUpSprites ();
	}

	public void option2() {
		Debug.Log ("option2: " + currentNode.getOption(2));
		currentNode = loader.getSceneNode (currentNode.getOption (2),
		                                   this.currentNode);
		setUpText ();
		setUpOptions ();
		setUpSprites ();
	}

	private void setUpText() {
		// Check if scene is over
		if (currentNode.getMessage() == "next_scene")
		{
			endScene = true;
		}

		if (!endScene) {
			convoText.text = currentNode.getMessage ();
			if (currentNode.isRightTalking () &&
			    !rightNameText.text.Equals(currentNode.getCharName ())) {
				rightNameText.text = currentNode.getCharName ();
			}
			else if (!currentNode.isRightTalking() &&
			         !leftNameText.text.Equals(currentNode.getCharName ())) {
				leftNameText.text = currentNode.getCharName();
			}
		}
	}

	private void setUpSprites() {
		if (endScene) return;
		if (this.loadedSprites == null) {
			this.loadedSprites = new Dictionary<string, Sprite>();
		}
		// Left
		if (this.curLeftSpriteName == null ||
		    !this.curLeftSpriteName.Equals(currentNode.getLeftSpriteFilename())) {
			Sprite left = null;
			
			// Use saved from dict
			if (this.loadedSprites.ContainsKey(currentNode.getLeftSpriteFilename())){
				left = loadedSprites[currentNode.getLeftSpriteFilename()];
			}
			// Load sprite
			else {
			// Get left sprite
			left = Resources.Load<Sprite>("Sprites/" +
	                                      currentNode.getLeftSpriteFilename());
			// Don't want a null
			if (left == null)
				Debug.LogError("Loaded null left sprite: " + currentNode.getLeftSpriteFilename());
				this.loadedSprites.Add(currentNode.getLeftSpriteFilename(),
			    	                   left);
			}

			GameObject leftRenderer = GameObject.Find("LeftSprite");
			leftRenderer.GetComponent<SpriteRenderer>().sprite = left;
		}

		// Right
		if (this.curRightSpriteName == null ||
		    !this.curRightSpriteName.Equals(currentNode.getRightSpriteFilename())) {
			Sprite right = null;

			// Use saved from dict
			if (this.loadedSprites.ContainsKey(currentNode.getRightSpriteFilename())){
				right = loadedSprites[currentNode.getRightSpriteFilename()];
			}
			// Load sprite
			else {
				// Get right sprite
				right = Resources.Load<Sprite>("Sprites/" +
				                              currentNode.getRightSpriteFilename());
				// Don't want a null
				if (right == null)
					Debug.LogError("Loaded null right sprite: " + currentNode.getRightSpriteFilename());
				this.loadedSprites.Add(currentNode.getRightSpriteFilename(),
				                       right);
			}

			GameObject rightRenderer = GameObject.Find("RightSprite");
			rightRenderer.GetComponent<SpriteRenderer>().sprite = right;
		}
	}

	private void setUpOptions() {
		if (endScene) return;
		int i = 0;
		for (; i < currentNode.getOptionCount(); ++i) {
			// Make next button invisible
			if (currentNode.getOptionText(i) == "next")
			{
				Color oldNextC = buttons[i].GetComponent<Image>().color;
				Color nextC = new Color(oldNextC.r, oldNextC.g, oldNextC.b, 0);
				buttons[i].GetComponent<Image>().color = nextC;
				buttons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 2000);
				buttons[i].GetComponent<Button>().interactable = true;
				buttons[i].GetComponentInChildren<Text>().text = "";
			}
			else
			{
				Color oldC = buttons[i].GetComponent<Image>().color;
				Color c = new Color(oldC.r, oldC.g, oldC.b, 255);
				buttons[i].GetComponent<Image>().color = c;
				buttons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
				buttons[i].GetComponent<Button>().interactable = true;
				buttons[i].GetComponentInChildren<Text>().text = currentNode.getOptionText(i);
			}
		}
		for (; i < buttons.Count; ++i) {
			Color oldC = buttons[i].GetComponent<Image>().color;
			Color c = new Color(oldC.r, oldC.g, oldC.b, 0);
			buttons[i].GetComponent<Image>().color = c;
			buttons[i].GetComponent<Button>().interactable = false;
			buttons[i].GetComponentInChildren<Text>().text = currentNode.getOptionText(i);
		}
		
	}
}
