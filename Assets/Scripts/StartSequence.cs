using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSequence : MonoBehaviour {
	public GameObject player1LockInObject;
	public GameObject player2LockInObject;
	public GameObject player3LockInObject;
	public GameObject player4LockInObject;

	private string[,] playerControlMappings;
	public int reptilePlayerIndex {get; private set;}
	public int controllerCount {get; private set;}
	private int lockedInCount = 0;

	public static StartSequence Instance { get; private set; }

	void Awake() {
		if(Instance != null && Instance != this)
		{
			Destroy(this);
		}

		Instance = this;
		DontDestroyOnLoad(this);
	}

	void Start() {
		// This only runs once! OnLevelWasLoaded handles future plays
		Init();
	}

	private void Init() {
		controllerCount = Input.GetJoystickNames().Length;
		playerControlMappings = new string[controllerCount, 2];

		for(int i = 0; i < controllerCount; i++) {
			// Assign control names... blah
			playerControlMappings[i,0] = "Player "+(i+1)+" Attack";
			playerControlMappings[i,1] = "Player "+(i+1)+" Attack 2";
		}

		int reptilePlayer = UnityEngine.Random.Range(0, controllerCount);
		Debug.Log("COUNT: "+controllerCount+" reptile: "+reptilePlayer);
	}
	
	void Update () {
		for(int playerIndex = 0; playerIndex < controllerCount; playerIndex++) {
			if(Input.GetButtonDown(playerControlMappings[playerIndex,0])){
				TryLockIn(playerIndex, true);
			} else if(Input.GetButtonDown(playerControlMappings[playerIndex,1])){
				TryLockIn(playerIndex, false);
			}
		}
	}

	private bool TryLockIn(int playerIndex, bool AButton) {
		if(AButton) {
			if(playerIndex == reptilePlayerIndex) {
				return false;
			} else {
				NotifyLockIn(playerIndex);
				return true;
			}
		} else {
			if(playerIndex == reptilePlayerIndex) {
				NotifyLockIn(playerIndex);
				return true;
			} else {
				return false;
			}
		}
	}

	private void NotifyLockIn(int playerIndex) {
		switch(playerIndex) {
		case 0: 
			player1LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		case 1: 
			player2LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		case 2: 
			player3LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		case 3: 
			player4LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		default:
			Debug.LogError("Unknown controller locked in...");
			break;
		}
		
	}

	void OnLevelWasLoaded(int level) {
		Debug.Log("test");
		if(level == SceneManager.GetSceneByName("StartScene").buildIndex) {
			reptilePlayerIndex = -1;
			Init();
		}
	}

	
}
