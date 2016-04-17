using UnityEngine;
using System.Collections;

public class Monster : PlayerChar {
	public float timeUntilTransform = 10;
	public Sprite lizardForm, humanForm;
	private float feedTimer;
	private bool transformed = false;
	public AudioSource slurpSound;

	public override void Start ()
	{
		base.Start();
		feedTimer = timeUntilTransform;
	}

	void Update(){
		ProcessMovement();
//		if(Input.GetButtonDown(attackButtonName)){
//			Action();
//		}
		if(Input.GetKeyDown(ButtonReference.AButtonKeyCode(this.PlayerNumber))) {
			Action();
		}
		if(!transformed){
			feedTimer -= 1/60f;
			if(feedTimer <= 0 ){
				sr.sprite = lizardForm;
				transformed = true;
			}
		}
	}

	public override void Action (){
		Debug.Log("Attack");
		RaycastHit2D hit = Physics2D.Raycast(transform.position ,directionVector,3);

		if(hit.transform != null){
//			Debug.Log(hit.transform.gameObject.name);
			IAttackable victim = hit.transform.gameObject.GetComponent<IAttackable>();
			if(victim != null){
				feedTimer = 10;
				if(transformed){
					sr.sprite = humanForm;
					transformed = false;
				}
				sr.color = hit.transform.gameObject.GetComponent<SpriteRenderer>().color;
				victim.Attacked(this, 1);
				this.slurpSound.Play();
			}
			
		}
	}
}
