using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {
	public AudioClip intensityOneSong;
	public AudioClip intensityTwoSong;
	public AudioClip intensityThreeSong;
	public AudioClip gameOverMusic;
	private int currIntensityLevel = 1;
	private int MaxIntensity = 3;
	public AudioSource source;

	// Update is called once per frame
	bool civLevel1Triggered = false;
	bool civLevel2Triggered = false;
	bool intensityLevelChanged = false;
	bool gameEndTriggered = false;
	int monsterDeathsRecognized = 0;
	void Update () {
		if(GameController.Instance.DoneSpawning()) {
			if(GameController.Instance.CivilianPercentageLeft() < 0.5 && !civLevel1Triggered) {
				Debug.Log("CIV LEVEL 1 TRIGGERED");
				civLevel1Triggered = true;
				intensityLevelChanged = true;
				currIntensityLevel++;
				if(currIntensityLevel > MaxIntensity) {
					currIntensityLevel = MaxIntensity;
				}
			}

			if(GameController.Instance.CivilianPercentageLeft() < 0.2 && !civLevel2Triggered) {
				Debug.Log("CIV LEVEL 2 TRIGGERED");
				civLevel2Triggered = true;
				intensityLevelChanged = true;
				currIntensityLevel++;
				if(currIntensityLevel > MaxIntensity) {
					currIntensityLevel = MaxIntensity;
				}
			}

			int monstersDied = GameController.Instance.StartingPlayerCount() - 1 - GameController.Instance.lizards.Count;
			if(monstersDied > monsterDeathsRecognized) {
				Debug.Log("MONSTER DEATH TRIGGERED: STARTING: "+GameController.Instance.StartingPlayerCount() + " lizards: "+GameController.Instance.lizards.Count);
				monsterDeathsRecognized++;
				intensityLevelChanged = true;
				currIntensityLevel++;
				if(currIntensityLevel > MaxIntensity) {
					currIntensityLevel = MaxIntensity;
				}
			}
		}

		if(GameController.Instance.IsGameOver() && !gameEndTriggered) {
			intensityLevelChanged = false;
			gameEndTriggered = true;
			source.clip = gameOverMusic;
			source.Play();
		}
		if(intensityLevelChanged) {
			Debug.Log("CHANGED INTENSE");
			intensityLevelChanged = false;
			switch(currIntensityLevel) {
			case 1: 
				source.clip = intensityOneSong;
				break;
			case 2:
				source.clip = intensityTwoSong;
				break;
			case 3:
				source.clip = intensityThreeSong;
				break;
			default:
				Debug.LogError("Unknown audio intensity level.");
				break;
			}

			source.Play();
		}
	}
}
