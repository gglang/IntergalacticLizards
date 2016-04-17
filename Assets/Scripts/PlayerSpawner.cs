using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
	public GameObject monsterPrefab;
	public GameObject hunterPrefab;
	public GameObject spawnLocation;
	public string PlayerLockInInfoObjectName = "PlayerLockInInfo";
	public float SpawnTime = 5f;

	private float nextSpawn = 0;
	private int playerNumberSpawning = 1;

	private PlayerLockInInfo playerInfo;
	void Start() {
		GameObject info = GameObject.Find(PlayerLockInInfoObjectName);
		playerInfo = info.GetComponent<PlayerLockInInfo>();
	}

	void Update () {
		if(Time.time > nextSpawn) {
			nextSpawn = Time.time + SpawnTime;

			if(playerNumberSpawning == playerInfo.hunterPlayerNumber) {
				SpawnPlayer(playerNumberSpawning, true);
			} else {
				SpawnPlayer(playerNumberSpawning, false);
			}

			playerNumberSpawning++;
			if(playerNumberSpawning > playerInfo.controllerCount) {
				Destroy(this);
			}
		}
	}

	private void SpawnPlayer(int playerNumber, bool hunter) {
		if(playerNumber < 1 || playerNumber > 4) {
			Debug.LogError("Unknown player detected when spawning");
			return;
		}

		GameObject playerObject;
		if(hunter) {
			playerObject = (GameObject) Instantiate(hunterPrefab, spawnLocation.transform.position, Quaternion.identity);
		} else {
			playerObject = (GameObject) Instantiate(monsterPrefab, spawnLocation.transform.position, Quaternion.identity);
		}
		PlayerChar controls = playerObject.GetComponent<PlayerChar>();
		controls.SetPlayerNumber(playerNumber);
//		controls.horizontalAxisName = "Player "+(playerIndex+1)+" Horizontal";
//		controls.verticalAxisName = "Player "+(playerIndex+1)+" Vertical";
//		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
//			controls.attackButtonName = "Player "+(playerIndex+1)+" Attack";
//		#elif UNITY_STANDALONE_WIN	
//			controls.attackButtonName = "Windows Player "+(playerIndex+1)+" Attack";
//		#endif
	}
}
