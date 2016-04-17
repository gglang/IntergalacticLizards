using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MonsterNavMove))]
public class MovementController : MonoBehaviour, IAttackable {

	[Range(2,100)]
	public int minTargets = 2; 
	[Range(2,100)]
	public int maxTargets = 5;

	public int maxWounds = 1;
	protected int currentWounds;

	private MonsterNavMove mover;
	private IList<GameObject> targets;

	void Start () {
		mover = GetComponent<MonsterNavMove>();
		targets = new List<GameObject>();

		StartCoroutine(PickTargets());
		currentWounds = maxWounds;
	}

	private IEnumerator PickTargets() {
		yield return new WaitForFixedUpdate();
		string[] targetLayers = {"Target"};
		if(GameController.Instance == null) {
			Debug.Log("WTF");
		}
		IList<GameObject> targettables = GameController.Instance.FindTrackedObjectsInLayers(LayerMask.GetMask(targetLayers));

		if(maxTargets < minTargets) {
			maxTargets = minTargets;
		}

		if(maxTargets > targettables.Count) {
			maxTargets = targettables.Count;
		}

		if(minTargets > targettables.Count) {
			minTargets = targettables.Count;
		}

		int targetAmount = UnityEngine.Random.Range(minTargets, maxTargets+1);
		int targetsAcquired = 0;
		while(targetsAcquired < targetAmount) {
			int index = UnityEngine.Random.Range(0, targettables.Count);
			targets.Add(targettables[index]);
			targetsAcquired++;
		}
	}
	private bool moved = false;
	private int targetIndex = 0;
	// Update is called once per frame
	void Update () {
		if(targets == null || targets.Count <= 0) {
			return;
		}

		if(!moved) {
			moved = true;
			mover.StartMove(targets[targetIndex].transform.position);
		}

		if(mover.ReachedDestination && moved) {
			targetIndex = (targetIndex + 1)%targets.Count;
			moved = false;
			mover.StopMove();
		}
	}

	public virtual void Attacked(PlayerChar attacker, int wound){
		currentWounds -= wound;
		if(currentWounds <= 0){
			GameObject.Destroy(gameObject);
		}
	}
}
