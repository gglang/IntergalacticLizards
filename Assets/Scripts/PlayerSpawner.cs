using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
	public GameObject monsterPrefab;
	public GameObject hunterPrefab;
	public GameObject[] randomSpawnLocations;
	public string PlayerLockInInfoObjectName = "PlayerLockInInfo";
	public float SpawnTime = 5f;

	private float nextSpawn = 0;
	private int playerNumberSpawning = 1;

	private PlayerLockInInfo playerInfo;
	void Start() {
		GameObject info = GameObject.Find(PlayerLockInInfoObjectName);
		playerInfo = info.GetComponent<PlayerLockInInfo>();
	}

	private bool doneSpawning = false;
	public bool DoneSpawning() {
		return doneSpawning;
		//		if(playerNumberSpawning > playerInfo.controllerCount) {
//			Debug.Log("DONE SPAWNING: "+playerNumberSpawning + " controllers: "+playerInfo.controllerCount);
//			return true;
//		}
//		return false;
	}

	public int PlayerCount() {
		return playerInfo.controllerCount;
	}

	void Update () {
//		if(Time.time > nextSpawn && !DoneSpawning()) {
//			nextSpawn = Time.time + SpawnTime;
		while(playerNumberSpawning <= playerInfo.controllerCount) {
			if(playerNumberSpawning == playerInfo.hunterPlayerNumber) {
				SpawnPlayer(playerNumberSpawning, true);
			} else {
				SpawnPlayer(playerNumberSpawning, false);
			}

			playerNumberSpawning++;
			Debug.Log("SPAWNED: "+playerNumberSpawning);
		}
//		}

//		if(playerNumberSpawning > playerInfo.controllerCount) {
			StartCoroutine(SignalDoneSpawning());
//		}
	}

	private IEnumerator SignalDoneSpawning() {
		yield return new WaitForSeconds(1f);
		doneSpawning = true;
	}

	private void SpawnPlayer(int playerNumber, bool hunter) {
		if(playerNumber < 1 || playerNumber > 4) {
			Debug.LogError("Unknown player detected when spawning");
			return;
		}
//		int spawnIndex = (int)Mathf.Lerp(0f, (float)randomSpawnLocations.Length, UnityEngine.Random.value);
//		if(spawnIndex >= randomSpawnLocations.Length) {
//			spawnIndex = randomSpawnLocations.Length - 1;
//		}
		GameObject spawnLocation = randomSpawnLocations[playerNumber-1];
		GameObject playerObject;
		if(hunter) {
			playerObject = (GameObject) Instantiate(hunterPrefab, spawnLocation.transform.position, Quaternion.identity);
		} else {
			playerObject = (GameObject) Instantiate(monsterPrefab, spawnLocation.transform.position, Quaternion.identity);
		}
		PlayerChar controls = playerObject.GetComponent<PlayerChar>();
		controls.SetPlayerNumber(playerNumber);
	}
}
