using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FirstLoadUp : MonoBehaviour {
	public float PauseTime = 3.0f;
	void Start () {
		StartCoroutine(SwitchToStartScene());
	}
	
	private IEnumerator SwitchToStartScene() {
		yield return new WaitForSeconds(PauseTime);
		SceneManager.LoadScene("StartScene");
	}
}
