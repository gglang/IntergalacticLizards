using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSequence : MonoBehaviour {
	public bool buildForMac = true;
	public GameObject player1LockInObject;
	public GameObject player2LockInObject;
	public GameObject player3LockInObject;
	public GameObject player4LockInObject;
	private GameObject playerLockInInfo;
	public string PlayerLockInInfoObjectName = "PlayerLockInInfo";

	private PlayerLockInInfo info;
//	private string[,] playerControlMappings;
	private int lockedInCount = 0;

	public static StartSequence Instance { get; private set; }

//	void Awake() {
//		Debug.Log("IM WAKING");
//		if(Instance != null && Instance != this)
//		{
//			Destroy(this.gameObject);
//		}
//
//		Instance = this;
//		DontDestroyOnLoad(this);
//	}

	private int controllerCount;
	private int hunterPlayerNumber;
	void Start() {
		playerLockInInfo = GameObject.Find(PlayerLockInInfoObjectName);
		info = playerLockInInfo.GetComponent<PlayerLockInInfo>();
		Init();
	}

	private void Init() {
		controllerCount = Input.GetJoystickNames().Length;
//		playerControlMappings = new string[controllerCount, 2];

//		for(int i = 0; i < controllerCount; i++) {
			// Assign control names... blah
//			if(buildForMac) {
//				playerControlMappings[i,0] = "Player "+(i+1)+" Attack";
//				playerControlMappings[i,1] = "Player "+(i+1)+" Attack 2";
//			} else {
//				playerControlMappings[i,0] = "Windows Player "+(i+1)+" Attack";
//				playerControlMappings[i,1] = "Windows Player "+(i+1)+" Attack 2";
//			}
//		}

		// Randomly determine who is the hunter! Note that this strange way of randomizing is because Unity is bad
		float[] playerRandomValues = new float[controllerCount];
		for(int i = 0; i < playerRandomValues.Length; i++) {
			playerRandomValues[i] = UnityEngine.Random.value;
		}

		float shortestStraw = 2f;
		int shortestStawPlayerIndex = -1;
		for(int i = 0; i < playerRandomValues.Length; i++) {
			if(playerRandomValues[i] < shortestStraw) {
				shortestStraw = playerRandomValues[i];
				shortestStawPlayerIndex = i;
			}
		}

		if(shortestStawPlayerIndex < 0 || shortestStawPlayerIndex > 3) {
			Debug.LogError("Invalid hunter player number chosen.");
			return;
		}
		hunterPlayerNumber = shortestStawPlayerIndex + 1;
	}
	
	void Update () {
		if(lockedInCount == controllerCount) {
			info.controllerCount = controllerCount;
			info.hunterPlayerNumber = hunterPlayerNumber;
			SceneManager.LoadScene("Reptiloids");
			lockedInCount++;
		} else if(lockedInCount > controllerCount) {
			// Scene loading!
			return;
		}

		for(int playerNumber = 1; playerNumber <= controllerCount; playerNumber++) {
			if(Input.GetKeyDown(ButtonReference.AButtonKeyCode(playerNumber))){
				TryLockIn(playerNumber, true);
			} else if(Input.GetKeyDown(ButtonReference.BButtonKeyCode(playerNumber))){
				TryLockIn(playerNumber, false);
			}
		}
	}

	private bool TryLockIn(int playerNumber, bool AButton) {
		if(AButton) {
			if(playerNumber == hunterPlayerNumber) {
				return false;
			} else {
				NotifyLockIn(playerNumber);
				lockedInCount++;
				return true;
			}
		} else {
			if(playerNumber == hunterPlayerNumber) {
				NotifyLockIn(playerNumber);
				lockedInCount++;
				return true;
			} else {
				return false;
			}
		}
	}

	private void NotifyLockIn(int playerNumber) {
		switch(playerNumber) {
		case 1: 
			player1LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		case 2: 
			player2LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		case 3: 
			player3LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		case 4: 
			player4LockInObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
			break;
		default:
			Debug.LogError("Unknown controller locked in...");
			break;
		}
		
	}

//	void OnLevelWasLoaded(int level) {
//		Debug.Log("test");
//		if(level == SceneManager.GetSceneByName("StartScene").buildIndex) {
//			hunterPlayerIndex = -1;
//			lockedInCount = 0;
//			Init();
//		}
//	}

	
}
