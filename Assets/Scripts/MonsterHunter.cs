using UnityEngine;
using System.Collections;

public class MonsterHunter : PlayerChar {
	void Update(){
		ProcessMovement();
//		if(Input.GetButtonDown(attackButtonName)){
//			Action();
//		}
//		if(Input.GetButtonDown("Fire1")) {
//			Action();
//		}
		if(Input.GetKeyDown(ButtonReference.AButtonKeyCode(this.PlayerNumber))) {
			Action();
		}
	}
}
