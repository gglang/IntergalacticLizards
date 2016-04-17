using UnityEngine;
using System.Collections;
//using System.Diagnostics;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;

public class MonsterNavMove : MonoBehaviour {
	public float StartMoveSpeed = 1.0f;
	public float RepathRate = 1;	//0 means no-auto repath
	public bool DirectionalMoveAnim = false;
	public bool RotationalMoveAnim = false;
	public float MoveNextDist = 0.1f;
	public float StoppingDistance = 0.3f;
	public float stuckFactor = 0.05f;

	private Seeker pathSeeker;
	private Animator animController;
	private RVOController2D rvoController;

	private bool searchingPath;
	private float nextRepath;
	private float normalSpeed;
	private float currentSpeed;
	private bool stopped;
	private Path path;
	private int currentWaypoint;
	private Vector3 destinationVector;
	private GameObject destinationObject;
	private Vector3 stopPosition;	//RVO simulation might push the object away a bit

	private float lastStuckCheckTime;

	private void Awake() {
		normalSpeed = StartMoveSpeed;
		currentSpeed = StartMoveSpeed;
		pathSeeker = GetComponent<Seeker>();
		rvoController = GetComponent<RVOController2D>();
	}

	private void Start() {
		animController = gameObject.GetComponent<Animator>();
		currentWaypoint = 0;
		stopped = true;
		searchingPath = false;
		stopPosition = transform.position;
		ReachedDestination = false;
	}
	 
	/* Returns true if the last point in the path was reached*/
	private bool reachedDestination;
	public bool ReachedDestination {
		get {
			if(!searchingPath) {
				return reachedDestination;
			} else {
				return false;
			}
		}

		private set {
			reachedDestination = value;
		}
	}

	public bool IsMoving {get {return !stopped;} }

	public bool LastWaypointReached() {
		return path != null && currentWaypoint == path.vectorPath.Count - 1;
	}

	public void ChangeSpeedByRatio(float ratio) {
		currentSpeed *= ratio;
	}

	public void ResetSpeed() {
		currentSpeed = normalSpeed;
	}

	public void StopMove() {
		if(!stopped) {
//			if(animController != null && (DirectionalMoveAnim || RotationalMoveAnim)) {
//				animController.SetBool ("Moving", false);// TODO A helper class should handle the setting of animations so that the mover doesn't need to know about the Animator
//			}

			//Updating graph so other units must path around my stopped location
			this.BlockArea(transform.position, rvoController.radius*2);

			stopPosition = transform.position;
			stopped = true;
			ReachedDestination = false;
			destinationObject = null;
			rvoController.Move (Vector3.zero);
		}
	}

	private void BlockArea(Vector2 position, float radius) {
		Bounds b = new Bounds(position, new Vector3(radius, radius, 0));
		GraphUpdateObject t = new GraphUpdateObject(b);
		t.setWalkability = false;
		t.modifyWalkability = true;
		if(AstarPath.active != null) {
			AstarPath.active.UpdateGraphs(t);
		}
	}

	private void UnblockArea(Vector2 position, float radius) {
		Bounds b = new Bounds(position, new Vector3(radius, radius, 0));
		GraphUpdateObject t = new GraphUpdateObject(b);
		t.setWalkability = true;
		t.modifyWalkability = true;

		if(AstarPath.active != null) {
			AstarPath.active.UpdateGraphs(t);
		}
	}

	public void StartMove(Vector2 target) {
		if(stopped) {
			if(animController == null) {
//				animController = this.GetComponent<Animator>();	// HACK because of weird bug with first instance of beast not getting this in Start()
			}
			if(animController != null && (DirectionalMoveAnim || RotationalMoveAnim)) {
//				animController.SetBool ("Moving", true); // TODO A helper class should handle the setting of animations so that the mover doesn't need to know about the Animator
			}
				
			//Updating graph so that stop location is no longer blocked
			this.UnblockArea(stopPosition, rvoController.radius*2);

			ReachedDestination = false;
			stopped = false;
			destinationVector = new Vector3(target.x, target.y, this.transform.position.z); // FIXME this is a weird hack due to our unconventional use of dimensions
			RecalculatePath();
			lastStuckCheckTime = Time.fixedTime;
		} else {
			Debug.Log("WHAT THE FUCK!??!?!?!?!?!?");
		}
	}

	public void StartMove(GameObject target) {
		destinationObject = target;
		this.StartMove(target.transform.position);
	}

	public void PauseMove() {
		if(!stopped) {
			if(animController != null && (DirectionalMoveAnim || RotationalMoveAnim)) {
				animController.SetBool ("Moving", false);// TODO A helper class should handle the setting of animations so that the mover doesn't need to know about the Animator
			}
			stopped = true;
		}
	}

	public void ResumeMove() {
		StartMove(destinationVector);
	}

	public void RecalculatePath () {
		searchingPath = true;
		nextRepath = Time.time+RepathRate;
		if(destinationObject != null) {
			destinationVector = destinationObject.transform.position;
		}
		pathSeeker.StartPath (transform.position,destinationVector,OnPathComplete);
	}

	public void OnPathComplete ( Path p )
	{
		ABPath abp = p as ABPath;
		searchingPath = false;

		if (path != null) path.Release (this);
		path = p;
		p.Claim (this);

		if (p.error) {
			currentWaypoint = 0;
			path = null;
			return;
		}
			
		Vector3 p1 = abp.originalStartPoint;
		Vector3 p2 = transform.position;
		float d = (p2-p1).magnitude;
		currentWaypoint = 0;

		Vector3 waypoint;

		for (float t=0;t<=d;t+=MoveNextDist*0.6f) {
			currentWaypoint--;
			Vector3 pos = p1 + (p2-p1)*t;

			do {
				currentWaypoint++;
				waypoint = p.vectorPath[currentWaypoint];
			} while ((pos - waypoint).sqrMagnitude < MoveNextDist*MoveNextDist && currentWaypoint != p.vectorPath.Count-1);

		}
	}
		
	private Queue<Vector2> changeQueue = new Queue<Vector2>();

	public void Update () {

		if (stopped || path == null || searchingPath) {	// TODO TODO TODO BUG TO FIX!
			return;
		}

		if (currentWaypoint == path.vectorPath.Count-1 && (transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < StoppingDistance * StoppingDistance) {
			ReachedDestination = true;
			rvoController.Move(Vector3.zero);
			return;
		}

		if (RepathRate != 0 && Time.time >= nextRepath && !searchingPath) {
//			RecalculatePath ();
		}

		if(IsStuck()) { //FIXME should this be turned on?
			RecalculatePath();
		}

		Vector3 dir = Vector3.zero;
		Vector3 waypoint = path.vectorPath[currentWaypoint];
		if ((transform.position - waypoint).sqrMagnitude > StoppingDistance * StoppingDistance) {
			if (path != null && path.vectorPath.Count != 0) {
				while (currentWaypoint != path.vectorPath.Count-1 && (transform.position - waypoint).sqrMagnitude < MoveNextDist*MoveNextDist) {
					currentWaypoint++;
					waypoint = path.vectorPath[currentWaypoint];
				}
					
				dir = (waypoint - transform.position).normalized * currentSpeed;
			}
		} 
			
		rvoController.Move (dir);

		changeQueue.Enqueue(dir.normalized);	

		Vector3 averagedDir = new Vector3(0,0, this.transform.position.z);	// TODO A helper class should handle the setting of animations so that the mover doesn't need to know about the Animator
		foreach(Vector2 change in changeQueue) {
			averagedDir.x += change.x;
			averagedDir.y += change.y;		// HACK this is here to prevent erratic animation when the unit is trying to find its way out of a pack, may cause lag
		}
		float scaler = 1f/( (float)changeQueue.Count );
		averagedDir.Scale(new Vector2(scaler, scaler));

		if (dir.sqrMagnitude != 0) {
			if(DirectionalMoveAnim) {
				HandleDirectionalAnim(dir.normalized);
			} else {
				HandleRotationalAnim(dir.normalized);
			}
		}
	}
		
	public Vector2 Destination { 
		get {
			return destinationVector;
		}

		private set {
			destinationVector = value;
		}
	}

	public void PushBack(Vector2 destination, float speed){
		StopMove();
		currentSpeed = speed;
	}

	private float StuckCheckCooldownTime = 0.5f;
	private bool gameJustLoaded = true;
	public bool IsStuck() {
		if(gameJustLoaded) {
			// FIXME this is a rough hack to get around stuck checks right on game load.
			lastStuckCheckTime = Time.fixedTime;
			gameJustLoaded = false;
			return false;
		}

		if(IsMoving) {
			float currTime = Time.fixedTime;
			if(currTime - lastStuckCheckTime < StuckCheckCooldownTime) {
				return false;	// FIXME not sure if this timer is a robust way to handle this
			}

			lastStuckCheckTime = currTime;

			float currSpeed = rvoController.velocity.magnitude;
			if(currSpeed < rvoController.maxSpeed * stuckFactor) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	/** The value of the animation's natural rotation. */
	public float RotationalAnimOffset = 0f;

	private void HandleRotationalAnim(Vector2 direction) {
//		float angle = Utilities.Instance.DirectionToAngle(direction) - RotationalAnimOffset;
//		this.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));
	}

	private void HandleDirectionalAnim(Vector2 direction) {
//		animController.SetFloat ("XDiff", direction.x);
//		animController.SetFloat ( "YDiff", direction.y);	// TODO A helper class should handle the setting of animations so that the mover doesn't need to know about the Animator
	}

//	void OnDestroy() {
//		if(stopped) {
//			this.UnblockArea(stopPosition, rvoController.radius*2);
//		}
//	}

	/// <summary>
	/// Call this to clean up and prepare for death.
	/// </summary>
	public void HandleDeath() {
		if(stopped) {
			this.UnblockArea(stopPosition, rvoController.radius*2);
		}
	}
}
