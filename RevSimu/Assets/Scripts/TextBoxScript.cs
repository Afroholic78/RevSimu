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

	private bool fadeOut = false; // Fade out boolean. If false: rightNameBox is visibile, leftNameBox is invisible
	private float duration = 1.0f; // Duration float used for fading
	public float speed = 0f; // Speed of fade


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

		SceneNode startNode = loader.getSceneNode (control.getStartNode ());
		if (startNode == null) {
			Debug.LogError("startNode is null");
		}

		// Find and associate text box components
		GameObject convoGO = GameObject.Find("ConvoText");
		GameObject convoBackgroundGO = GameObject.Find("ConvoBackground");
		convoBackground = convoBackgroundGO.GetComponent <Image> ();
		convoText = convoGO.GetComponent <Text> ();
		convoText.text = startNode.getMessage ();

		GameObject rightNameGO = GameObject.Find("RightNameText");
		GameObject rightNameBackgroundGO = GameObject.Find("RightNameBackground");
		rightNameBackground = rightNameBackgroundGO.GetComponent <Image> ();
		rightNameText = rightNameGO.GetComponent <Text> ();

		GameObject leftNameGO = GameObject.Find("LeftNameText");
		GameObject LeftNameBackgroundGO = GameObject.Find("LeftNameBackground");
		leftNameBackground = LeftNameBackgroundGO.GetComponent <Image> ();
		leftNameText = leftNameGO.GetComponent <Text> ();
		leftNameText.text = startNode.getCharName ();

	}
	
	// Update is called once per frame
	void Update () {
		//convoText.text = "LOL THIS IS ACTUALLY WORKING!";
		//rightNameText.text = "Ladeh";
		//leftNameText.text = "Bloke";
	}
}
