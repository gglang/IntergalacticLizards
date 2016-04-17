using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Vector2 direction;
	public float velocity = 0;
	public PlayerChar shooter = null;
	public int damage = 2;
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.right * direction.x * velocity * Time.deltaTime + Vector3.up * direction.y * velocity * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other){
		IAttackable victim = other.GetComponent<IAttackable>();
		if(victim != null){
			victim.Attacked(shooter,damage);
		}
		GameObject.Destroy(gameObject);
	}
}
