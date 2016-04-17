using UnityEngine;
using System.Collections;

public class PlayerLockInInfo : MonoBehaviour {

	public int controllerCount {get; set;}
	public int hunterPlayerNumber {get; set;}

	// Use this for initialization
	public static PlayerLockInInfo Instance { get; private set; }

	void Awake () {
		if(Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}

		Instance = this;
		DontDestroyOnLoad(this);
	}
}
