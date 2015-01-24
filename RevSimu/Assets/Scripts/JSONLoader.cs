using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class JSONLoader {

	private JSONNode node = null;
	public JSONLoader(string fileName) {
		// http://answers.unity3d.com/questions/761628/create-a-glossary-using-simple-json-loading-data-d.html

		TextAsset textAss = Resources.Load<TextAsset> (fileName);

		this.node = SimpleJSON.JSONNode.Parse(textAss.text);
		if (this.node == null)
						Debug.LogError ("Node(" + fileName + ") was null!");

		//Debug.Log (node["First test"]);

	}

	public SceneNode getSceneNode(string nodeName) {
		string name = this.node [nodeName] ["name"];
		string message = this.node [nodeName] ["message"];
		string backgroundFile = this.node [nodeName] ["backgound"];
		string charName = this.node [nodeName] ["charName"];

		// Get out of the JSONArray and put it into a different collection
		JSONArray a = this.node [nodeName] ["options"].AsArray;
		List<string> options = new List<string> ();

		for (int i = 0; i < a.Count; ++i) {
			options.Add (a[i]);
		}
		return new SceneNode (name, message, backgroundFile, charName, options);
	}

}

public class SceneNode {
	private string sceneName = null;
	private string charName = null;
	private string message = null;
	private List<string> options = null;
	private string backgroundFile = null;

	public SceneNode(string sceneName, string message, string backgroundFile,
	                 string charName, List<string> options) {
		this.sceneName = sceneName;
		this.message = message;
		this.backgroundFile = backgroundFile;
		this.charName = charName;
		this.options = options;
	}

	public string getName() {
		return this.sceneName;
	}

	public string getMessage() {
		return this.message;
	}

	public string getOption(int index) {
		if (index >= this.options.Count) {
			return null;
		}
		return options[index];
	}

	public string getBackgroundFilename() {
		return this.backgroundFile;
	}

	public string getCharName() {
		return this.charName;
	}
}