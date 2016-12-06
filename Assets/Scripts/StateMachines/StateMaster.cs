using UnityEngine;
using System.Collections;

public class StateMaster : MonoBehaviour {

	public StateMachine plantDry, plantFallow, plantGrown, plantGrowing, plantPlanted, plantWithered;
	public StateMachine animalRoaming, animalCrying, animalFollowing, animalRunningAway;
	public InputMachine inputUI, inputTeleport, inputInteract
	, inputPickUp, inputInspect;
	public HouseMachine houseOpen;
	public GameObject number, timer;

	public static StateMaster instance;

	public void Awake(){
		StateMaster.instance = this;
		plantDry = gameObject.AddComponent<Plant_Dry>();
		plantFallow = gameObject.AddComponent<Plant_Fallow>();
		plantGrowing = gameObject.AddComponent<Plant_Growing>();
		plantGrown = gameObject.AddComponent<Plant_Grown>();
		plantPlanted = gameObject.AddComponent<Plant_Planted>();
		plantWithered = gameObject.AddComponent<Plant_Withered>();

		animalCrying = gameObject.AddComponent<Animal_Crying>();
		animalFollowing = gameObject.AddComponent<Animal_Following>();
		animalRoaming = gameObject.AddComponent<Animal_Roaming>();
		animalRunningAway = gameObject.AddComponent<Animal_RunningAway>();

		inputUI = gameObject.AddComponent<Input_UI>();
		inputTeleport = gameObject.AddComponent<Input_Teleport>();
		inputInteract = gameObject.AddComponent<Input_Interact>();
		inputPickUp = gameObject.AddComponent<Input_PickUp>();
		inputInspect = gameObject.AddComponent<Input_Inspect>();

		houseOpen = gameObject.AddComponent<House_Open> ();
	}
}
