using UnityEngine;
using System.Collections;

/// <summary>
/// This causes this gameobject to tell the GameController when it Starts, and when it is Destroyed; this makes allows for higher performance queries on the state
/// objects in the game.
/// </summary>
public class TrackedByGameController : MonoBehaviour {
	void Start () {
		GameController.Instance.BirthInTheFamily(this.gameObject);
	}

	void OnDestroy() {
		GameController.Instance.DeathInTheFamily(this.gameObject);
	}
}
