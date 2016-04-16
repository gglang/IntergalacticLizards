using UnityEngine;
using System.Collections;

public class MonsterHunter : PlayerChar {
	void Update(){
		ProcessMovement();
		if(Input.GetButtonDown(attackButtonName)){
			Action();
		}
	}
}
