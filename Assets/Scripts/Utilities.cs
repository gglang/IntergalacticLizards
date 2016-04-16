using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utilities : MonoBehaviour {
	private IList<GameObject> targets;

	// Singleton
	public static Utilities Instance { get; private set; }

	void Awake() {
		if(Instance != null && Instance != this)
		{
			Destroy(this);
		}

		Instance = this;

		targets = new List<GameObject>();
	}

	public void TargetBorn(GameObject target) {
		targets.Add(target);
	}

	public void TargetDied(GameObject target) {
		targets.Remove(target);
	}

	public IList<GameObject> GetTargetables() {
		return targets;
	}
}
