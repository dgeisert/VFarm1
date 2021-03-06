﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMachine : StateMachine {

	public static GameObject playerObject;
	public static PlayerMachine instance;
	public InputMachine inputMachine;
	private Dictionary<string, int> resources;
	private Dictionary<string, GameObject> loadedResources;

	public override void InstanceInitiate(StateMachine checkMachine){
		GetComponent<StateMaster> ().Setup ();
		inputMachine.Initiate ();
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
		if (ES2.Exists ("playerLocation")) {
			Transform tr = ES2.Load<Transform> ("playerLocation");
			transform.position = tr.position;
			transform.localScale = tr.localScale;
			transform.localRotation = tr.localRotation;
			Destroy (tr.gameObject);
		} else {
			transform.position = AreaStartStateMachine.instance.transform.position + Vector3.up * InputMachine.playerHeight;
			transform.rotation = AreaStartStateMachine.instance.transform.rotation;
		}
		LoadGos ();
		if (InputMachine.instance.gos.Count == 0) {
			ADMIN.CreateGos ();
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
	public void SavePlayerPosition(){
		ES2.Save (transform, "playerLocation");
	}
	public void SaveResources(){
		SavePlayerPosition ();
		ES2.Save(resources, "resources");
	}
	public void SaveGos(){
		InputMachine.instance.gos.RemoveAll (item => item == null);
		Dictionary<Transform, SaveObject> saveGos = new Dictionary<Transform, SaveObject> ();
		foreach (GameObject go in InputMachine.instance.gos) {
			StateMachine sm = go.GetComponent<StateMachine> ();
			saveGos.Add (go.transform, new SaveObject(sm));
		}
		ES2.Save(saveGos, "gos");
	}
	public void LoadGos(){
		if (InputMachine.instance.gos == null) {
			InputMachine.instance.gos = new List<GameObject> ();
		}
		InputMachine.instance.gos.RemoveAll (item => item == null);
		if (ES2.Exists ("gos")) {
			Dictionary<Transform, SaveObject> loadGos = ES2.LoadDictionary<Transform, SaveObject> ("gos");
			foreach (KeyValuePair<Transform, SaveObject> kvp in loadGos) {
				if (loadedResources.ContainsKey (kvp.Value.objName)) {
					GameObject go = (GameObject)GameObject.Instantiate (loadedResources [kvp.Value.objName]);
					go.GetComponent<StateMachine> ().Load(kvp.Key, kvp.Value);
					Destroy (kvp.Key.gameObject);
				}
			}
			InputMachine.instance.CheckObjects ();
		}
	}
}
