using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GenAI))]
[RequireComponent(typeof(PlayerSpawner))]
public class GameController : MonoBehaviour {
	private IDictionary<int, List<GameObject>> trackedGameobjectsByLayer;
	public GameObject GameOverScreen;
	public float MinimumWaitTimeBeforeGameOver = 30f;

	// Singleton
	public static GameController Instance { get; private set; }
	private PlayerSpawner spawner;
	private GenAI aiGenerator;
	private bool InitComplete = false;
	void Awake() {
		if(Instance != null && Instance != this)
		{
			Destroy(this);
		}

		Instance = this;

		trackedGameobjectsByLayer = new Dictionary<int, List<GameObject>>();
		spawner = GetComponent<PlayerSpawner>();
		aiGenerator = GetComponent<GenAI>();
	}

	public IList<GameObject> civilians;
	public IList<GameObject> lizards;
	public IList<GameObject> hunters;
	void Update() {
		if(!spawner.DoneSpawning()) {
			return;
		}
//		Debug.Log("Checking for game end");

		InitComplete = true;

		this.GetLists();
//		Debug.Log("civ: "+civilians.Count+" liz: "+lizards.Count+" hunt: "+hunters.Count);


		if(civilians.Count == 0) {
//			Debug.Log("NO CIVS");
			TriggerLizardWin();
		} else if(hunters.Count == 0) {
//			Debug.Log("NO HUNTS");
			TriggerLizardWin();
		} else if(lizards.Count == 0) {
//			Debug.Log("NO LIZ");
			TriggerHunterWin();
		}
	}

	private bool gameover = false;
	private void TriggerHunterWin() {
		if(!gameover) {
			GameOverScreen.SetActive(true);
			UnityEngine.UI.Text endText = GameOverScreen.GetComponentInChildren<UnityEngine.UI.Text>();
			endText.text = "MONSTER HUNTER WINS! INFOWARS.COM";
			gameover = true;
		}
	}

	private void TriggerLizardWin() {
		if(!gameover) {
			GameOverScreen.SetActive(true);
			UnityEngine.UI.Text endText = GameOverScreen.GetComponentInChildren<UnityEngine.UI.Text>();
			endText.text = "LIZARD ALIENS WIN! HILARY CLINTON FOR PREZ!";
			gameover = true;
		}
	}

	private void GetLists() {
		string[] civLayers = {"AI"};
		civilians = this.FindTrackedObjectsInLayers(LayerMask.GetMask(civLayers));

		string[] lizardLayers = {"Monster"};
		lizards = this.FindTrackedObjectsInLayers(LayerMask.GetMask(lizardLayers));

		string[] hunterLayers = {"Hunter"};
		hunters = this.FindTrackedObjectsInLayers(LayerMask.GetMask(hunterLayers));
	}

	public bool DoneSpawning() {
		return spawner.DoneSpawning() && InitComplete;
	}

	/// <summary>
	/// Returns fraction between 0 and 1
	/// </summary>
	/// <returns>The percentage left.</returns>
	public float CivilianPercentageLeft() {
		return ((float)this.civilians.Count) / ((float)aiGenerator.numberOfVillagers);
	}

	public int StartingPlayerCount() {
		return spawner.PlayerCount();
	}

	public bool IsGameOver() {
		return gameover;
	}

	/// <summary>
	/// This method should be called by every single GameObject that wishes to be tracked; at birth
	/// </summary>
	/// <param name="go">Go.</param>
	public void BirthInTheFamily(GameObject go) {
		if(trackedGameobjectsByLayer.ContainsKey(go.layer)) {
			List<GameObject> layerList = trackedGameobjectsByLayer[go.layer];
			if(layerList.Contains(go)) { 
				return; 
			}
			layerList.Add(go);
		} else {
			List<GameObject> newList = new List<GameObject>();
			newList.Add(go);
			trackedGameobjectsByLayer.Add(go.layer, newList);
		}
	}

	/// <summary>
	/// This method should be called by every single GameObject that wishes to be tracked; at death
	/// </summary>
	/// <param name="go">Go.</param>
	public void DeathInTheFamily(GameObject go) {
		if(trackedGameobjectsByLayer.ContainsKey(go.layer)) {
			List<GameObject> layerList = trackedGameobjectsByLayer[go.layer];
			if(layerList.Contains(go)) {
				layerList.Remove(go);
			}
		}
	}

	/**
	 * Return list of all GameObjects in a layer.
	 * Return empty list if no GameObjects in layer.
	 */
	public IList<GameObject> FindObjectsInLayers(LayerMask layers) {
		// TODO make a cached list of objects by layer; add and remove from list in awake and ondestroy methods?
		GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
		IList<GameObject> goList = new List<GameObject>();
		foreach (GameObject go in goArray) {
			if (this.IsInLayerMask(go, layers)) {
				goList.Add(go);
			}
		}
		return goList;
	}

	/// <summary>
	/// Finds the tracked objects in layers. Objects are tracked if they have the "Tracked" (***This is not yet implemented! - AbDeath is being used***) component on them.
	/// This will have much greater lookup performance than the standard method of looking up objects in a certain layer. 
	/// </summary>
	/// <returns>The tracked objects in layers.</returns>
	/// <param name="layers">Layers.</param>
	public IList<GameObject> FindTrackedObjectsInLayers(LayerMask layers) {	// TODO The "Tracked" Component is not yet implemented...
		List<GameObject> resultList = new List<GameObject>();

		int mask = layers.value;
		int currLayer = 1;
		int currLayerMask = 1 << currLayer;
		while(currLayerMask <= mask) {
			int result = currLayerMask & mask;
			if(result > 0) {
				if(trackedGameobjectsByLayer.ContainsKey(currLayer)) {
					resultList.AddRange(trackedGameobjectsByLayer[currLayer]);
				}
			}

			currLayer++; 
			currLayerMask = 1 << currLayer;
		}
		return resultList;
	}

	public bool IsInLayerMask(GameObject go, LayerMask mask) {
		int objLayerMask = (1 << go.layer);
		if ((mask.value & objLayerMask) > 0) {
			return true;
		} else {
			return false;
		}
	}

	public void RestartGame() {
		SceneManager.LoadScene("StartScene");
	}
}
