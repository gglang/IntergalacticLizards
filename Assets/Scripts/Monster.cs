using UnityEngine;
using System.Collections;

public class Monster : PlayerChar {
	void Update(){
		ProcessMovement();
		if(Input.GetButtonDown(attackButtonName)){
			Action();
		}
	}
}
