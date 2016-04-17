//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class Utilities : MonoBehaviour {
//	private GameObject GameManager;
//	private IDictionary<int, List<GameObject>> trackedGameobjectsByLayer;
//	public LayerMask FriendlyLayers;
//	public LayerMask EnemyLayers;
//
//	// Singleton
//	public static Utilities Instance { get; private set; }
//
//	void Awake() {
//		if(Instance != null && Instance != this)
//		{
//			Destroy(this);
//		}
//
//		Instance = this;
//
//		trackedGameobjectsByLayer = new Dictionary<int, List<GameObject>>();
//		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
//		guiCanvasObject = GameObject.FindGameObjectWithTag("GUICanvas");
//		GameManager = GameObject.FindGameObjectWithTag ("GameManager");
//	}
//
//	private Camera mainCam;
//	public Vector2 ScreenToWorldPoint(Vector2 point) {
//		return mainCam.ScreenToWorldPoint(point);
//	}
//
//	public GameObject GetGameManager() {
//		return GameManager;
//	}
//
//	/// <summary>
//	/// Returns point on a collider that ray cast hits. Throws UnityException if nothing is hit.
//	/// </summary>
//	/// <returns>Vector3 point of ray cast in world.</returns>
//	/// <param name="point">Vector2 point in screen space.</param>
//	public Vector3 ScreenToWorldRayCast(Vector2 point) {
//		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//		RaycastHit hit;
//		if (Physics.Raycast (ray, out hit, 100)) {
//			return hit.point;
//		} else {
//			throw new UnityException("Ray cast did not hit anything...");
//		}
//	}
//
//	public Vector2 WorldToScreenPoint(Vector2 point) {
//		return mainCam.WorldToScreenPoint(point);
//	}
//
//	public IList<GameObject> GetAllFriendlyTargets() {
//		return FindObjectsInLayers(FriendlyLayers);
//	}
//
//	private GameObject guiCanvasObject;
//	public GameObject GetGUICanvasObject() {
//		return guiCanvasObject;
//	}
//
//	public IList<GameObject> FindObjectsInLayersInRadius(Vector2 center, float radius, LayerMask layers) {
//		IList<GameObject> result = new List<GameObject>();
//		Collider2D[] nearbyInLayer = Physics2D.OverlapCircleAll(center, radius, layers);
//		foreach(Collider2D coll in nearbyInLayer) {
//			result.Add(coll.gameObject);
//		}
//		return result;
//	}
//
//	/* Return array of objects in a layer in specified quad */
//	public IList<GameObject> ObjectsInLayersInPoly(Vector3[] points, LayerMask layers) {
//		IList<GameObject> returnList = new List<GameObject> ();
//		IList<GameObject> objInLayer = Utilities.Instance.FindObjectsInLayers(layers);
//		if(objInLayer != null) {
//			foreach(GameObject obj in objInLayer) {
//				if (IsPointInPolygon(points, obj.transform.position)) {
//					returnList.Add(obj);
//				}
//			}
//		}
//	
//		return returnList;
//	}
//
//	public bool IsPointInPolygon(Vector3[] vertices, Vector2 point){
//		bool result = false;
//		for (int i = 0, j = vertices.Length - 1; i < vertices.Length; j = i++) {
//			if ((vertices[i].y > point.y) != (vertices[j].y > point.y) &&
//				(point.x < (vertices[j].x - vertices[i].x) * (point.y - vertices[i].y) / (vertices[j].y-vertices[i].y) + vertices[i].x)) {
//				result = !result;
//			}
//		}
//		return result;
//	}
//
//	/* Return array of SpellFX in radius of given center. Return empty list if no spellfx in radius. */
//	public IList<GameObject> SpellFXInRadius(Vector2 center, float radius) {
//		Collider2D[] nearbySpells = Physics2D.OverlapCircleAll(center, radius, LayerMask.GetMask("SpellFX")); 
//		IList<GameObject> returnList = new List<GameObject>();
//		foreach(Collider2D spell in nearbySpells) {
//			returnList.Add (spell.gameObject);
//		}
//		return returnList;
//	}
//
//	/**
//	 * Return list of all GameObjects in a layer.
//	 * Return empty list if no GameObjects in layer.
//	 */
//	public IList<GameObject> FindObjectsInLayers(LayerMask layers) {
//		// TODO make a cached list of objects by layer; add and remove from list in awake and ondestroy methods?
//		GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
//		IList<GameObject> goList = new List<GameObject>();
//		foreach (GameObject go in goArray) {
//			if (this.IsInLayerMask(go, layers)) {
//				goList.Add(go);
//			}
//		}
//		return goList;
//	}
//
//	/// <summary>
//	/// Finds the tracked objects in layers. Objects are tracked if they have the "Tracked" (***This is not yet implemented! - AbDeath is being used***) component on them.
//	/// This will have much greater lookup performance than the standard method of looking up objects in a certain layer. 
//	/// </summary>
//	/// <returns>The tracked objects in layers.</returns>
//	/// <param name="layers">Layers.</param>
//	public IList<GameObject> FindTrackedObjectsInLayers(LayerMask layers) {	// TODO The "Tracked" Component is not yet implemented...
//		List<GameObject> resultList = new List<GameObject>();
//
//		int mask = layers.value;
//		int currLayer = 1;
//		int currLayerMask = 1 << currLayer;
//		while(currLayerMask <= mask) {
//			int result = currLayerMask & mask;
//			if(result > 0) {
//				if(trackedGameobjectsByLayer.ContainsKey(currLayer)) {
//					resultList.AddRange(trackedGameobjectsByLayer[currLayer]);
//				}
//			}
//
//			currLayer++; 
//			currLayerMask = 1 << currLayer;
//		}
//		return resultList;
//	}
//
//	public bool IsInLayerMask(GameObject go, LayerMask mask) {
//		int objLayerMask = (1 << go.layer);
//		if ((mask.value & objLayerMask) > 0) {
//			return true;
//		} else {
//			return false;
//		}
//	}
//
//	/**
//	 * Returns distance between 2 points in screen coordinates,
//	 * in centimeters of real world distance.
//	 */
//	public float ScreenDistanceCM(Vector2 p1, Vector2 p2) {
//		float distance = Vector2.Distance(p1, p2);
//		float hackDPI = 300f;//HACK
//		float distanceInch = distance / hackDPI/*Screen.dpi*/;	// FIXME is DPI always reliable? What is a good default? FIXME THIS DPI DOESNT APPEAR TO BE RELIABLE, AND WORK AROUNDS WERE HACKED IN PLACES WHERE IT IS USED - AT LEAST IT SEEMS TO AFFECT UNITY REMOTE FIXME
//		return distanceInch * 2.54f;
//	}
//
//	/*
//	 * Returns the angle of a direction (magnitude 1), to Vector2(0,1).
//	 * This is an improvement on Vector2.Angle, seeing as it doesn't tolerate
//	 * angles > 180 degrees.
//	 * 
//	 * Returns float between 0 (inclusive) and 360 (exclusive).
//	 */
//	public float DirectionToAngle(Vector2 direction) {
//		Vector2 parsedDirection = direction;
//		float parseOffset = 0f;
//		if(direction.y < 0) {
//			// Angle is in 3rd or 4th quadrants, >180 degrees
//			parsedDirection = new Vector2(-direction.x, -direction.y);
//			parseOffset = 180f;
//		}
//
//		float angle = Vector2.Angle(parsedDirection, new Vector2(1, 0));
//		angle += parseOffset;
//		return angle;
//	}
//
//	public GameObject[] GetReferencedDisabledPlayerObjects() {
//		GameObject reference = GameObject.Find ("PlayerDisabledObjectReference");
//		DisabledObjectReference referenceScript = reference.GetComponent<DisabledObjectReference>();
//		return referenceScript.GetDisabled();
//	}
//
//	 private Texture2D lineTex;
//
//	public void DrawLine(Rect rect) { DrawLine(rect, GUI.contentColor, 1.0f); }
//	public void DrawLine(Rect rect, Color color) { DrawLine(rect, color, 1.0f); }
//	public void DrawLine(Rect rect, float width) { DrawLine(rect, GUI.contentColor, width); }
//	public void DrawLine(Rect rect, Color color, float width) { DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width); }
//	public void DrawLine(Vector2 pointA, Vector2 pointB) { DrawLine(pointA, pointB, GUI.contentColor, 1.0f); }
//	public void DrawLine(Vector2 pointA, Vector2 pointB, Color color) { DrawLine(pointA, pointB, color, 1.0f); }
//	public void DrawLine(Vector2 pointA, Vector2 pointB, float width) { DrawLine(pointA, pointB, GUI.contentColor, width); }
//	public void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
//	{
//		Matrix4x4 matrix = GUI.matrix;
//
//		if (!lineTex) { lineTex = new Texture2D(1, 1); }
//
//		Color savedColor = GUI.color;
//		GUI.color = color;
//
//		float angle = Vector3.Angle(pointB - pointA, Vector2.right);
//
//		if (pointA.y > pointB.y) { angle = -angle; }
//
//		GUIUtility.RotateAroundPivot(angle, pointA);
//
//		GUI.DrawTexture(new Rect(pointA.x, pointA.y, (pointB - pointA).magnitude, width), lineTex);
//
//		GUI.matrix = matrix;
//		GUI.color = savedColor;
//	}
//
//	private Texture2D _RectTexture;
//	private GUIStyle _RectStyle;
//
//	public void GUIDrawRect( Rect position, Color color )
//	{
//		if (_RectTexture == null) {
//			_RectTexture = new Texture2D (1, 1);
//		}
//
//		if (_RectStyle == null) {
//			_RectStyle = new GUIStyle ();
//		}
//
//		_RectTexture.SetPixel (0, 0, color);
//		_RectTexture.Apply();
//
//		_RectStyle.normal.background = _RectTexture;
//
//		GUI.Box (position, GUIContent.none, _RectStyle);
//	}
//
//	public Rect ScreenToGUIRect(Rect rect){
//		return new Rect(rect.x, Screen.height - (rect.y + rect.height), rect.width, rect.height);
//	}
//
//	/// <summary>
//	/// This method should be called by every single GameObject that wishes to be tracked; at birth
//	/// </summary>
//	/// <param name="go">Go.</param>
//	public void BirthInTheFamily(GameObject go) {
//		if(trackedGameobjectsByLayer.ContainsKey(go.layer)) {
//			List<GameObject> layerList = trackedGameobjectsByLayer[go.layer];
//			if(layerList.Contains(go)) { 
//				return; 
//			}
//			layerList.Add(go);
//		} else {
//			List<GameObject> newList = new List<GameObject>();
//			newList.Add(go);
//			trackedGameobjectsByLayer.Add(go.layer, newList);
//		}
//	}
//
//	/// <summary>
//	/// This method should be called by every single GameObject that wishes to be tracked; at death
//	/// </summary>
//	/// <param name="go">Go.</param>
//	public void DeathInTheFamily(GameObject go) {
//		if(trackedGameobjectsByLayer.ContainsKey(go.layer)) {
//			List<GameObject> layerList = trackedGameobjectsByLayer[go.layer];
//			if(layerList.Contains(go)) {
//				layerList.Remove(go);
//			}
//		}
//	}
//}