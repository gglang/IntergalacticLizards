using UnityEngine;
using System.Collections;

public class Targetable : MonoBehaviour {
	public string TargetID = "Default";

	void Start() {
		Utilities.Instance.TargetBorn(this.gameObject);
	}

	void OnDestroy() {
		Utilities.Instance.TargetDied(this.gameObject);
	}
}
