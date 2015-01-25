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
	}

	public SceneNode getSceneNode(string nextNodeName, SceneNode oldNode) {
		SceneNode next = new SceneNode (nextNodeName, this.node, oldNode);
		if (next.getSkipMe () && !next.getMessage().Equals("next_scene")) {
			if(next.getEmptyOption() == null)
				Debug.LogError("Empty emptyOptionInfo on skipme: " + next.getName() +
				               ", old: " + oldNode.getName() +
				               ", new next: " + next.getEmptyOption());
			
			return new SceneNode(next.getEmptyOption(), this.node, oldNode); 
		}
		return next;
	}

}

public class SceneNode {
	private string sceneName = null;
	private string charName = null;
	private string message = null;
	private string leftSpriteFile = null;
	private string rightSpriteFile = null;
	private bool isRight = false;
	private List<string> options = null;
	private List<string> optionsText = null;
	private List<string> usedOptions = null;
	private bool saveOptions = false;
	private bool skipMe = false;
	private string emptyOption = null;

	public SceneNode(string nextNodeName, JSONNode jsonNode, SceneNode prevNode) {
		this.sceneName = nextNodeName;
		this.message = jsonNode [nextNodeName] ["message"];
		this.leftSpriteFile = jsonNode [nextNodeName] ["leftSprite"];
		this.rightSpriteFile = jsonNode [nextNodeName] ["rightSprite"];
		this.charName = jsonNode [nextNodeName] ["charName"];
		this.isRight = jsonNode [nextNodeName] ["isRight"].AsBool;
		
		this.saveOptions = jsonNode [nextNodeName] ["addUsedOption"].AsBool;
		this.emptyOption = jsonNode [nextNodeName] ["emptyOption"];

		if(saveOptions || (prevNode != null && prevNode.getSaveOptions())) {
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
			Debug.LogError("Unequal amount of options/destinations for " + this.sceneName);

		if (this.options.Count == 0) {
			skipMe = true;
		}
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

	public string getLeftSpriteFilename() {
		return this.leftSpriteFile;
	}

	public string getRightSpriteFilename() {
		return this.rightSpriteFile;
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

	// Only the loader should touch this function
	public bool getSkipMe() {
		return this.skipMe;
	}
	public string getEmptyOption() {
		return this.emptyOption;
	}

	public bool isRightTalking() {
		return this.isRight;
	}
}