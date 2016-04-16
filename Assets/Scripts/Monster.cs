using UnityEngine;
using System.Collections;

public class Monster : PlayerChar {
	
	void Update(){
		ProcessMovement();
		if(Input.GetButtonDown(attackButtonName)){
			Action();
		}
	}

	public override void Action (){
		Debug.Log("Attack");
		RaycastHit2D hit = Physics2D.Raycast(transform.position ,directionVector,3);

		if(hit.transform != null){
//			Debug.Log(hit.transform.gameObject.name);
			IAttackable victim = hit.transform.gameObject.GetComponent<IAttackable>();
			if(victim != null){
				sr.color = hit.transform.gameObject.GetComponent<SpriteRenderer>().color;
				victim.Attacked(this, 1);
			}
			
		}
	}
}
