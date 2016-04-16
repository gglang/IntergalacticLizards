using UnityEngine;
using System.Collections;

public class GenAI : MonoBehaviour {
	public GameObject AI;
	public int numberOfVillagers = 30;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < numberOfVillagers;i++){
			GameObject g = GameObject.Instantiate(AI, new Vector3(Random.Range(-40,52),Random.Range(40,-50)), Quaternion.identity) as GameObject;
			int colorroll = Random.Range(0,5);
			SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
			switch(colorroll){
				case 0:
					sr.color = Color.white;
					break;
				case 1:
					sr.color = Color.blue;
					break;
				case 2:
					sr.color = Color.green;
					break;
				case 3:
					sr.color = Color.red;
					break;
				case 4:
					sr.color = Color.yellow;
					break;
			}
		}
	}


}

