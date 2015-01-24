using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class ConvoBoxScript : MonoBehaviour {


	private bool LoadFromFile(string filename) {
		// Function used to read text from a file to display
		// inside our conversation/dialogue box.
		
		try
		{
			string line;
			
			StreamReader reader = new StreaReader(filename, Encoding.Default); // create StreamReader object to read given file
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

		catch (Exception e)
		{
			Console.WriteLine("{0}\n", e.Message);
			return false;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
