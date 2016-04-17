using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
	public GameObject monsterPrefab;
	public GameObject hunterPrefab;
	public GameObject spawnLocation;
	public string PlayerLockInInfoObjectName = "PlayerLockInInfo";
	public float SpawnTime = 5f;

	private float nextSpawn = 0;
	private int spawnCount = 0;

	private PlayerLockInInfo playerInfo;
	void Start() {
		GameObject info = GameObject.Find(PlayerLockInInfoObjectName);
		playerInfo = info.GetComponent<PlayerLockInInfo>();
	}

	void Update () {
		if(Time.time > nextSpawn) {
			nextSpawn = Time.time + SpawnTime;

			if(spawnCount == playerInfo.hunterPlayerIndex) {
				SpawnPlayer(spawnCount, true);
			} else {
				SpawnPlayer(spawnCount, false);
			}

			spawnCount++;
			if(spawnCount >= playerInfo.controllerCount) {
				Destroy(this);
			}
		}
	}

	private void SpawnPlayer(int playerIndex, bool hunter) {
		GameObject playerObject;
		if(hunter) {
			playerObject = (GameObject) Instantiate(hunterPrefab, spawnLocation.transform.position, Quaternion.identity);
		} else {
			playerObject = (GameObject) Instantiate(monsterPrefab, spawnLocation.transform.position, Quaternion.identity);
		}
		PlayerChar controls = playerObject.GetComponent<PlayerChar>();
		controls.horizontalAxisName = "Player "+(playerIndex+1)+" Horizontal";
		controls.verticalAxisName = "Player "+(playerIndex+1)+" Vertical";
		controls.attackButtonName = "Player "+(playerIndex+1)+" Attack";
	}
}
