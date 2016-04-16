//using UnityEngine;
//using System.Collections;
//using Pathfinding;
//using Pathfinding.RVO;
//
//[RequireComponent(typeof(Seeker))]
//[RequireComponent(typeof(RVOController2D))]
//public class DonaldController : MonoBehaviour {
//	private Seeker pathSeeker;
//	private RVOController2D rvoController;
//	public GameObject target;
//	private bool searchingPath = false;
//	private bool stopped = true;
//
//	void Start () {
//		pathSeeker = GetComponent<Seeker>();
//		rvoController = GetComponent<RVOController2D>();
//		this.StartMove(target.transform.position);
//	}
//
//	void Update() {
//
//		Vector3 dir = Vector3.zero;
//		Vector3 waypoint = path.vectorPath[currentWaypoint];
//		if ((transform.position - waypoint).sqrMagnitude > StoppingDistance * StoppingDistance) {
//			if (path != null && path.vectorPath.Count != 0) {
//				while (currentWaypoint != path.vectorPath.Count-1 && (transform.position - waypoint).sqrMagnitude < MoveNextDist*MoveNextDist) {
//					currentWaypoint++;
//					waypoint = path.vectorPath[currentWaypoint];
//				}
//
//				dir = (waypoint - transform.position).normalized * currentSpeed;
//			}
//		} 
//
//		rvoController.Move (dir);
//
//		changeQueue.Enqueue(dir.normalized);
//	}
//
//	public void StartMove(Vector2 target) {
//		if(stopped) {
//			if(animController == null) {
//				animController = this.GetComponent<Animator>();	// HACK because of weird bug with first instance of beast not getting this in Start()
//			}
//			if(animController != null && (DirectionalMoveAnim || RotationalMoveAnim)) {
//				animController.SetBool ("Moving", true); // TODO A helper class should handle the setting of animations so that the mover doesn't need to know about the Animator
//			}
//
//			//Updating graph so that stop location is no longer blocked
//			this.UnblockArea(stopPosition, rvoController.radius*2);
//
//			ReachedDestination = false;
//			stopped = false;
//			destinationVector = new Vector3(target.x, target.y, this.transform.position.z); // FIXME this is a weird hack due to our unconventional use of dimensions
//			RecalculatePath();
//			lastStuckCheckTime = Time.fixedTime;
//		} else {
//			Debug.Log("WHAT THE FUCK!??!?!?!?!?!?");
//		}
//	}
//
//	public void OnPathComplete ( Path p )
//	{
//		ABPath abp = p as ABPath;
//		searchingPath = false;
//
//		if (path != null) path.Release (this);
//		path = p;
//		p.Claim (this);
//
//		if (p.error) {
//			currentWaypoint = 0;
//			path = null;
//			return;
//		}
//
//		Vector3 p1 = abp.originalStartPoint;
//		Vector3 p2 = transform.position;
//		float d = (p2-p1).magnitude;
//		currentWaypoint = 0;
//
//		Vector3 waypoint;
//
//		for (float t=0;t<=d;t+=MoveNextDist*0.6f) {
//			currentWaypoint--;
//			Vector3 pos = p1 + (p2-p1)*t;
//
//			do {
//				currentWaypoint++;
//				waypoint = p.vectorPath[currentWaypoint];
//			} while ((pos - waypoint).sqrMagnitude < MoveNextDist*MoveNextDist && currentWaypoint != p.vectorPath.Count-1);
//
//		}
//	}
//}
