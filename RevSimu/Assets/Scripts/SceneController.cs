using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	// Use name of scene to choose different starting nodes..
	public string getStartNode() {
		string name = Application.loadedLevelName;
		Debug.Log("Getting start of " + name +  ".");

		if (name == "scene1") {
			return "gamestart";
		} else if (name == "TemplateScene") { // Debug here
			return "gamestart";
		}
		else if (name == "scene2"){
			return "node2-1start";
		}
		else if (name == "scene3"){
			return "node3-1start";
		}

		return null;
	}

	// If we need to start separating json files here would be where
	public string getJSONFilename() {
		string name = Application.loadedLevelName;
		Debug.Log("Getting jsonfile of " + name +  ".");
		
		if (name == "scene1") {
		//	return "gamestart";
		}

		return "Text/content";
	}
}
