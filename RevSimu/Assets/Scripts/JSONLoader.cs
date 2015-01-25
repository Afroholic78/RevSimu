using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class JSONLoader {

	private JSONNode node = null;
	public JSONLoader(string fileName) {
		// http://answers.unity3d.com/questions/761628/create-a-glossary-using-simple-json-loading-data-d.html

		TextAsset textAss = Resources.Load<TextAsset> (fileName);
		if (textAss == null)
			Debug.LogError ("Text asset failed load " + fileName + ".");

		this.node = SimpleJSON.JSONNode.Parse(textAss.text);
		if (this.node == null)
						Debug.LogError ("Node(" + fileName + ") was null!");

		//Debug.Log (node["First test"]);

	}

	public SceneNode getSceneNode(string nextNodeName, SceneNode oldNode) {
		return new SceneNode (nextNodeName, this.node, oldNode);
		/*if(a.Count != 0) {
			return new SceneNode (name, this.node, oldNode);
		}
		else {
			string emptyOption = this.node [nextNodeName] ["emptyOption"];
			string emptyOptionText = this.node [nextNodeName] ["emptyOptionText"];
			return new SceneNode(name, message, backgroundFile, charName,
			                     oldNode, usedOption, emptyOption, emptyOptionText);
		}*/
	}

}

public class SceneNode {
	private string sceneName = null;
	private string charName = null;
	private string message = null;
	private string backgroundFile = null;
	private string emptyOption = null;
	private string emptyOptionText = null;
	private List<string> options = null;
	private List<string> optionsText = null;
	private List<string> usedOptions = null;
	private bool saveOptions = false;
	private bool ignoreSavedOptions = false;

	public SceneNode(string nextNodeName, JSONNode jsonNode, SceneNode prevNode) {
		this.sceneName = nextNodeName;
		this.message = jsonNode [nextNodeName] ["message"];
		this.backgroundFile = jsonNode [nextNodeName] ["background"];
		this.charName = jsonNode [nextNodeName] ["charName"];
		
		this.saveOptions = jsonNode [nextNodeName] ["addUsedOption"].AsBool;

		if(saveOptions || (prevNode != null && prevNode.getSaveOptions())) {
			Debug.LogWarning("Save..");
			this.usedOptions = new List<string>();
			List<string> oldSaved = prevNode.getUsedOptions();
			if (oldSaved != null) this.usedOptions.AddRange(oldSaved);
			if(prevNode != null && prevNode.getSaveOptions()) {
				this.usedOptions.Add(nextNodeName);
			}
		}
		else if (jsonNode [nextNodeName] ["ignoreUsedOptions"].AsBool) {
			this.usedOptions = new List<string>();
			List<string> oldSaved = prevNode.getUsedOptions();
			if (oldSaved != null) this.usedOptions.AddRange(oldSaved);
			Debug.LogWarning("Save..");
		}

		// Get out of the JSONArray and put it into a different collection
		JSONArray a =  jsonNode [nextNodeName] ["options"].AsArray;
		options = new List<string> ();
		for (int i = 0; i < a.Count; ++i) {
			if(this.saveOptions && this.usedOptions != null && this.usedOptions.Contains(a[i])) continue;
			options.Add (a[i]);
		}

		// Get out of the JSONArray and put it into a different collection
		JSONArray a2 = jsonNode [nextNodeName] ["optionsText"].AsArray;
		optionsText = new List<string> ();
		for (int i = 0; i < a2.Count; ++i) {
			if(this.saveOptions && this.usedOptions != null && this.usedOptions.Contains(a[i])) continue;
			optionsText.Add (a2[i]);
		}

		// Bad number
		if(this.options.Count != this.optionsText.Count)
			Debug.LogWarning("Unequal amount of options/destinations for " + this.sceneName);
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

	public string getOptionText(int index) {
		if (index >= this.optionsText.Count) {
			return null;
		}
		return optionsText[index];
	}

	public int getOptionCount() {
		return options.Count;
	}

	public string getBackgroundFilename() {
		return this.backgroundFile;
	}

	public string getCharName() {
		return this.charName;
	}

	public List<string> getUsedOptions() {
		return usedOptions;
	}

	public bool getSaveOptions() {
		return this.saveOptions;
	}
}