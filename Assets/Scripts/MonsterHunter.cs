using UnityEngine;
using System.Collections;

public class MonsterHunter : PlayerChar {
	public AudioSource attackSound;
	public GameObject potassiumBullet;
	public int ammo = 3;
	private int ammoRemaining;
	public override void Start ()
	{
		base.Start ();
		ammoRemaining = ammo;
	}

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
		}if(Input.GetKeyDown(ButtonReference.BButtonKeyCode(this.PlayerNumber))){
			FireBullet();
		}

	}

	void FireBullet(){
		if(ammoRemaining > 0){
			GameObject b = GameObject.Instantiate(potassiumBullet, transform.position,Quaternion.identity) as GameObject;
			Bullet bullet = b.GetComponent<Bullet>();
			bullet.shooter = this;
			bullet.direction = new Vector2(horizontalAxis,verticalAxis);
			if(bullet.direction == Vector2.zero){
				bullet.direction = Vector2.up;
			}
			bullet.velocity = 50;
			ammoRemaining -= 1;
		}

	}

	public override void Action(){
		//	Debug.Log("Attack");
		RaycastHit2D hit = Physics2D.Raycast(transform.position ,directionVector,3);
		GameObject.Instantiate(attack, transform.position, Quaternion.identity);
		if(hit.transform != null){
			//			Debug.Log(hit.transform.gameObject.name);
			IAttackable victim = hit.transform.gameObject.GetComponent<IAttackable>();
			if(victim != null){
				attackSound.Play();
				victim.Attacked(this, 1);
			}
		}
	}
}
