using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
	public GameObject monsterPrefab;
	public GameObject hunterPrefab;
	public GameObject[] randomSpawnLocations;
	public string PlayerLockInInfoObjectName = "PlayerLockInInfo";
	public float SpawnTime = 5f;

	private float nextSpawn = 0;
	private int spawnCount = 0;

	private PlayerLockInInfo playerInfo;
	void Start() {
		GameObject info = GameObject.Find(PlayerLockInInfoObjectName);
		playerInfo = info.GetComponent<PlayerLockInInfo>();
	}

	public bool DoneSpawning() {
		if(playerNumberSpawning > playerInfo.controllerCount) {
			return true;
		}
		return false;
	}

	public int PlayerCount() {
		return playerInfo.controllerCount;
	}

	void Update () {
		if(Time.time > nextSpawn && !DoneSpawning()) {
			nextSpawn = Time.time + SpawnTime;

			if(spawnCount == playerInfo.hunterPlayerIndex) {
				SpawnPlayer(spawnCount, true);
			} else {
				SpawnPlayer(spawnCount, false);
			}

			playerNumberSpawning++;
		}
	}

	private void SpawnPlayer(int playerNumber, bool hunter) {
		if(playerNumber < 1 || playerNumber > 4) {
			Debug.LogError("Unknown player detected when spawning");
			return;
		}
		int spawnIndex = (int)Mathf.Lerp(0f, (float)randomSpawnLocations.Length, UnityEngine.Random.value);
		if(spawnIndex >= randomSpawnLocations.Length) {
			spawnIndex = randomSpawnLocations.Length - 1;
		}
		GameObject spawnLocation = randomSpawnLocations[spawnIndex];
		GameObject playerObject;
		if(hunter) {
			playerObject = (GameObject) Instantiate(hunterPrefab, spawnLocation.transform.position, Quaternion.identity);
		} else {
			playerObject = (GameObject) Instantiate(monsterPrefab, spawnLocation.transform.position, Quaternion.identity);
		}
		PlayerChar controls = playerObject.GetComponent<PlayerChar>();
		controls.horizontalAxisName = "Player "+(playerIndex+1)+" Horizontal";
		controls.verticalAxisName = "Player "+(playerIndex+1)+" Vertical";
		controls.SetPlayerNumber(playerIndex + 1);
	}
}
