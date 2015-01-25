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

	public Text rightNameText; // reference to text component in right name box
	public Text leftNameText; // reference to text component in left name box
	public Text convoText; // reference to text component in dialogue box
	
	public Image rightNameBackground; // reference to background image in right image box
	public Image leftNameBackground; // reference to background image in left image box
	public Image convoBackground; // reference to backgroun image in dialogue box

	private bool fadeOut = false; // Fade out boolean. If false: rightNameBox is visibile, leftNameBox is invisible
	private float duration = 1.0f; // Duration float used for fading
	public float speed = 0f; // Speed of fade

    private bool toggle = true;


	private bool LoadFromFile(string filename) {
		// Function used to read text from a file to display
		// inside our conversation/dialogue box.
		// NOT USED AT THE MOMENT.
		
		try
		{
			string line;
			
			StreamReader reader = new StreamReader(filename, Encoding.Default); // create StreamReader object to read given file
			using(reader) // clean up the reader
			{ 
				do 
				{
					line = reader.ReadLine(); // read line
					if (line != null) 
					{	
						// Update GUI conversation box here
						Debug.Log(line); // print line to debug console
					} 
				}
				
				while (line != null);
				reader.Close(); // close reader when we're done reading
				return true;
			}
		}
		
		catch (System.Exception e)
		{
			Debug.Log(e.ToString());
			return false;
		}
	}

	private void FadeOut(GameObject imageGO) {
		// Function makes the passed GameObject fade out.
		// Will be called during click and timer events

		Color textureColor = renderer.material.color; // Color object, used for alpha channel
		if (fadeOut)
		{
			textureColor.a = duration - Mathf.PingPong(Time.time + speed, duration) / duration;
			renderer.material.color = textureColor;
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

		this.currentNode = loader.getSceneNode (control.getStartNode ());
		if (currentNode == null) {
			Debug.LogError("startNode is null");
		}

		// Find and associate text box components
		GameObject convoGO = GameObject.Find("ConvoText");
		GameObject convoBackgroundGO = GameObject.Find("ConvoBackground");
		convoBackground = convoBackgroundGO.GetComponent <Image> ();
		convoText = convoGO.GetComponent <Text> ();
		convoText.text = currentNode.getMessage ();

		GameObject rightNameGO = GameObject.Find("RightNameText");
		GameObject rightNameBackgroundGO = GameObject.Find("RightNameBackground");
		rightNameBackground = rightNameBackgroundGO.GetComponent <Image> ();
		rightNameText = rightNameGO.GetComponent <Text> ();

		GameObject leftNameGO = GameObject.Find("LeftNameText");
		GameObject LeftNameBackgroundGO = GameObject.Find("LeftNameBackground");
		leftNameBackground = LeftNameBackgroundGO.GetComponent <Image> ();
		leftNameText = leftNameGO.GetComponent <Text> ();
		leftNameText.text = currentNode.getCharName ();


		buttons = new List<GameObject> ();
		buttons.Add(GameObject.Find("OptionButton0"));
		buttons.Add(GameObject.Find("OptionButton1"));
		buttons.Add(GameObject.Find("OptionButton2"));
		setUpOptions ();
		//buttons[0].AddComponent<SpeechBubbleFloat>();
	}
	
	// Update is called once per frame
	void Update () {
		//convoText.text = "LOL THIS IS ACTUALLY WORKING!";
		//rightNameText.text = "Ladeh";
		//leftNameText.text = "Bloke";
        Invoke("fade", 2);
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
		currentNode = loader.getSceneNode (currentNode.getOption (0));
		convoText.text = currentNode.getMessage ();
		leftNameText.text = currentNode.getCharName ();
		setUpOptions ();
	}

	
	
	public void option1() {
		Debug.Log ("option1: " + currentNode.getOption(1));
		currentNode = loader.getSceneNode (currentNode.getOption (1));
		convoText.text = currentNode.getMessage ();
		leftNameText.text = currentNode.getCharName ();
		setUpOptions ();
	}

	public void option2() {
		Debug.Log ("option2: " + currentNode.getOption(2));
		currentNode = loader.getSceneNode (currentNode.getOption (2));
		convoText.text = currentNode.getMessage ();
		leftNameText.text = currentNode.getCharName ();
		setUpOptions ();
	}

	private void setUpOptions() {
		int i = 0;
		for (; i < currentNode.getOptionCount(); ++i) {
			Color oldC = buttons[i].GetComponent<Image>().color;
			Color c = new Color(oldC.r, oldC.g, oldC.b, 255);
			buttons[i].GetComponent<Image>().color = c;
			buttons[i].GetComponent<Button>().interactable = true;
			buttons[i].GetComponentInChildren<Text>().text = currentNode.getOptionText(i);

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
