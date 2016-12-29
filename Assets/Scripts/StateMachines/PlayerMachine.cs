using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMachine : StateMachine {

	public static GameObject playerObject;
	public static PlayerMachine instance;
	private Dictionary<string, int> resources;
	private Dictionary<string, GameObject> loadedResources;

	void Awake () {
		PlayerMachine.playerObject = gameObject;
		PlayerMachine.instance = this;
		DontDestroyOnLoad (gameObject);
		if (ES2.Exists ("resources")) {
			resources = ES2.LoadDictionary<string, int> ("resources");
		} else {
			resources = new Dictionary<string, int> ();
		}
		Object[] objs = Resources.LoadAll ("");
		loadedResources = new Dictionary<string, GameObject> ();
		foreach (Object obj in objs) {
			if (obj.name.Substring (0, 3) == "obj") {
				loadedResources.Add (obj.name, (GameObject)obj);
			}
		}
	}

	public int GetResource(string resource){
		resource = resource.ToLower ();
		if(!resources.ContainsKey(resource)) {
			CreateResource (resource);
		}
		return resources [resource];
	}
	private void CreateResource(string resource){
		resources.Add (resource, 0);
	}
	public void SetResource(string resource, int count){
		resource = resource.ToLower ();
		if(!resources.ContainsKey(resource)) {
			CreateResource (resource);
		}
		resources [resource] = count;
		SaveResources ();
		SaveGos ();
	}
	public void AddResource(string resource, int increment){
		resource = resource.ToLower ();
		if(!resources.ContainsKey(resource)) {
			CreateResource (resource);
		}
		resources [resource] += increment;
		SaveResources ();
		SaveGos ();
	}
	public void SaveResources(){
		ES2.Save(resources, "resources");
	}
	public void SaveGos(){
		InputMachine.instance.gos.RemoveAll (item => item == null);
		Dictionary<Transform, string> saveGos = new Dictionary<Transform, string> ();
		foreach (GameObject go in InputMachine.instance.gos) {
			saveGos.Add (go.transform, go.name);
		}
		ES2.Save(saveGos, "gos");
	}
	public void LoadGos(){
		if (ES2.Exists ("gos")) {
			Dictionary<Transform, string> loadGos = ES2.LoadDictionary<Transform, string> ("gos");
			foreach (KeyValuePair<Transform, string> kvp in loadGos) {
				if (loadedResources.ContainsKey (kvp.Value)) {
					GameObject go = (GameObject)GameObject.Instantiate (loadedResources [kvp.Value], kvp.Key.position, kvp.Key.rotation);
					go.transform.localScale = kvp.Key.localScale;
					InputMachine.instance.gos.Add (go);
				}
			}
			InputMachine.instance.CheckObjects ();
		}
	}
}
