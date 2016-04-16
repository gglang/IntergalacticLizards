using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerChar : MonoBehaviour, IAttackable {
	public float speed = 4;
	public int maxWounds = 2;
	public string horizontalAxisName, verticalAxisName, attackButtonName;
	protected float horizontalAxis, verticalAxis;
	protected Vector2 directionVector = Vector2.zero;
	protected float directionX, directionY;
	protected int currentWounds;

	void Start(){
		currentWounds = maxWounds;
	}

	public virtual void ProcessMovement(){
//		if(Mathf.Sign(Input.GetAxis(horizontalAxisName)) * horizontalAxis <= 0 && Input.GetAxis(horizontalAxisName) != 0){
//			directionVector = new Vector2(Mathf.Sign(Input.GetAxis(horizontalAxisName)),directionVector.y);
//			Debug.Log("horizontal change");
//		}
//		if(Mathf.Sign(Input.GetAxis(verticalAxisName)) * verticalAxis <= 0 && Input.GetAxis(verticalAxisName) != 0){
//			directionVector = new Vector2(directionVector.x, Mathf.Sign(Input.GetAxis(verticalAxisName)));
//			Debug.Log("vertical change");
//		}
		
		horizontalAxis = Input.GetAxis(horizontalAxisName);
		verticalAxis = Input.GetAxis(verticalAxisName);
		directionX = horizontalAxis == 0 ? directionX:horizontalAxis;
		directionY = verticalAxis == 0 ? directionY:verticalAxis;
		directionVector = new Vector2(directionX,directionY);
		Debug.DrawRay(transform.position,directionVector);
	//	Debug.Log("Horizontal Axis: " + horizontalAxis + " Vertical Axis: " + verticalAxis);
		transform.position += (Vector3.right * speed * Time.deltaTime * horizontalAxis) + (Vector3.up * speed * Time.deltaTime * verticalAxis);
		transform.localScale = new Vector3(Mathf.Sign(directionX),1);
	}

	public virtual void Action(){
	//	Debug.Log("Attack");
		RaycastHit2D hit = Physics2D.Raycast(transform.position,directionVector,1);

		if(hit.transform != null){
//			Debug.Log(hit.transform.gameObject.name);
			IAttackable victim = hit.transform.gameObject.GetComponent<IAttackable>();
			if(victim != null){
				victim.Attacked(this, 1);
			}
			
		}
	}

	public virtual void Attacked(PlayerChar attacker, int wound){
		currentWounds -= wound;
		Debug.Log(this + " Attacked! " + wound + " wound");
		if(currentWounds <= 0){
			GameObject.Destroy(gameObject);
		}
	}
}
