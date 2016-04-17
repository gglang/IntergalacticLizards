using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerChar : MonoBehaviour, IAttackable {
	public float speed = 4;
	public GameObject attack;
	public int maxWounds = 2;
	public string horizontalAxisName, verticalAxisName, attackButtonName;
	protected float horizontalAxis, verticalAxis;
	protected Vector2 directionVector = Vector2.zero;
	protected float directionX, directionY;
	protected int currentWounds;
	protected SpriteRenderer sr;
	protected AudioSource aus;

	public GameObject DeadBody;

	public virtual void Start(){
		currentWounds = maxWounds;
		sr = gameObject.GetComponent<SpriteRenderer>();
		aus = gameObject.GetComponent<AudioSource>();
		int colorroll = Random.Range(0,5);
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

	public virtual void ProcessMovement(){
		horizontalAxis = Input.GetAxis(horizontalAxisName);
		verticalAxis = Input.GetAxis(verticalAxisName);
		directionX = horizontalAxis == 0 ? directionX:Mathf.Sign(horizontalAxis);
		directionY = verticalAxis == 0 ? directionY:Mathf.Sign(verticalAxis);
		directionVector = new Vector2(directionX,directionY);
		Debug.DrawRay(transform.position ,directionVector);
	//	Debug.Log("Horizontal Axis: " + horizontalAxis + " Vertical Axis: " + verticalAxis);
		transform.position += (Vector3.right * speed * Time.deltaTime * horizontalAxis) + (Vector3.up * speed * Time.deltaTime * verticalAxis);
//		if(horizontalAxis <  0)
//			transform.localScale = new Vector3(-3,3);
//		if(horizontalAxis > 0)
//			transform.localScale = new Vector3(3,3);
//		
	}

	public virtual void Action(){
	//	Debug.Log("Attack");
		RaycastHit2D hit = Physics2D.Raycast(transform.position ,directionVector,3);
		GameObject.Instantiate(attack, transform.position + new Vector3(Mathf.Sign(horizontalAxis) * 1.5f, Mathf.Sign(verticalAxis) * 1.5f), Quaternion.identity);
		if(hit.transform != null){
//			Debug.Log(hit.transform.gameObject.name);
			IAttackable victim = hit.transform.gameObject.GetComponent<IAttackable>();
			if(victim != null){
				victim.Attacked(this, 1);
			}
			
		}
	}

	public virtual void Attacked(PlayerChar attacker, int wound){
		aus.Play();
		currentWounds -= wound;
		Debug.Log(this + " Attacked! " + wound + " wound");
		if(currentWounds <= 0){
			Instantiate(DeadBody, this.transform.position, Quaternion.identity);
			GameObject.Destroy(gameObject);
		}
	}

	protected int PlayerNumber = -1;
	/// <summary>
	/// Takes player number between 1 (inclusive) and 4 (inclusive)
	/// </summary>
	/// <param name="playerNumber">Player number.</param>
	public void SetPlayerNumber(int playerNumber) {
		if(playerNumber < 1 || playerNumber > 4) {
			Debug.LogError("Unknown player number detected");
			return;
		}
		PlayerNumber = playerNumber;
		this.horizontalAxisName = "Player "+playerNumber+" Horizontal";
		this.verticalAxisName = "Player "+playerNumber+" Vertical";
	}
}
