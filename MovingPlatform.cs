using UnityEngine;


public class MovingPlatform : MonoBehaviour, IActivatable
{
	[Header("Settings")]
	public Transform[] waypoints;
	public float speed = 2.0f;
	public float waitTime = .5f;
	public bool startsActive = false;
	public LayerMask riderMask; 

	public Rigidbody2D platformRB;
	public Collider2D platformCollider;
	private Vector2 previousPosition;
	private int currentPoint;
	private int tabDirection = 1;
	private bool isActive;
	private float waitCounter;

	
	void Start()
	{
		isActive = startsActive;
		previousPosition = platformRB.position;

		foreach (Transform waypoint in waypoints)
		{
			waypoint.SetParent(null);// So the waypoint doesn't move with the parent
		}
	}

	
	public void Activate()
	{
		isActive = true;
	}

	void Update()
	{

		if (platformRB == null || platformCollider == null)
		{
			Debug.Log("MovingPlatform: No RB or Colloder");
			return;
		}
		Vector2 delta = platformRB.position - previousPosition;
		
		
		if (delta != Vector2.zero)
		{
			
			Bounds bounds = platformCollider.bounds;
			
			Vector2 center= new Vector2(bounds.center.x, bounds.max.y + .05f);
			Vector2 size = new Vector2(bounds.size.x * .9f, .1f);

			Collider2D[] riders = Physics2D.OverlapBoxAll(center, size, 0f, riderMask);
			foreach (Collider2D rider in riders)
			{
				if (rider.attachedRigidbody != null && rider.CompareTag("Player"))
					rider.attachedRigidbody.position += delta;
				
			}
		}
		previousPosition=platformRB.position;
		
		if (!isActive ||waypoints.Length == 0) return;

		if (waitCounter > 0)
		{
			waitCounter -= Time.fixedDeltaTime;
			return;
		}

		Vector2 target = waypoints[currentPoint].position;
		Vector2 next = Vector2.MoveTowards(platformRB.position, target, speed * Time.deltaTime);
		platformRB.MovePosition(next);

		if (Vector2.Distance(next, target) < .05f)
		{
			
			
			waitCounter = waitTime;
			currentPoint += tabDirection;
			if (currentPoint == waypoints.Length - 1) tabDirection = -1;
			if (currentPoint == 0) tabDirection = 1;
		}
	}
}
