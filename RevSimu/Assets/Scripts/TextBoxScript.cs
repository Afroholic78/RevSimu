using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.IO;

public class TextBoxScript : MonoBehaviour {
	
	public Text rightNameText; // reference to text component in right name box
	public Text leftNameText; // reference to text component in left name box
	public Text convoText; // reference to text component in dialogue box
	
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
	
	// Use this for initialization
	void Start () {
		// Find and associate text box components
		GameObject convoGO = GameObject.Find("ConvoText");
		GameObject convoCanvasGO = GameObject.Find("ConvoCanvas");
		convoText = convoGO.GetComponent <Text> ();

		GameObject rightNameGO = GameObject.Find("RightNameText");
		rightNameText = rightNameGO.GetComponent <Text> ();

		GameObject leftNameGO = GameObject.Find("LeftNameText");
		leftNameText = leftNameGO.GetComponent <Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		convoText.text = "LOL THIS IS ACTUALLY WORKING!";
		rightNameText.text = "Ladeh";
		leftNameText.text = "Bloke";
	}
}
