using UnityEngine;


public class SmartMissileController : MonoBehaviour
{
	[Header("Settings")]
	public float speed = 12.0f;

	public float turnSpeed = 220.0f;
	public int damage = 30;
	public float lifetime = 5.0f;

	[Header("References")]
	public Rigidbody2D missileRB;

	public GameObject impactFx;

	[HideInInspector] public Transform target;
	[HideInInspector] public Vector2 fallbackDir = Vector2.right;

	private Vector2 currentDir;


	void Start()
	{
		currentDir = (target != null) ? ((Vector2)target.position - (Vector2)transform.position).normalized : fallbackDir;
		Destroy(gameObject, lifetime);
	}


	void Update()
	{
		if (missileRB == null)
		{
			Debug.LogError("missileRB is null");
			return;
		}
		
		//revalidate the target each update
		bool hasValidTarget = false;
		if (target != null)
		{
			Enemy targetEnemy = target.GetComponent<Enemy>();
			hasValidTarget = targetEnemy != null && targetEnemy.isAlive;
		}

		Vector2 desiredDir = hasValidTarget ? ((Vector2)target.position - missileRB.position).normalized : currentDir;

		float angle = Vector2.SignedAngle(currentDir, desiredDir);
		float step = Mathf.Clamp(angle, -turnSpeed * Time.deltaTime, turnSpeed * Time.deltaTime);
		currentDir = Quaternion.Euler(0, 0, step) * currentDir;

		missileRB.MovePosition(missileRB.position + currentDir * speed * Time.deltaTime);
		transform.right = currentDir; //orient sprite alongside travel direction 
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<Enemy>().ReceiveDamage(damage);
			Destroy(gameObject);
		}
		else if (other.CompareTag("BlocType1"))
		{
			Properties blocProperties = other.GetComponent<Properties>();
			blocProperties.health -= damage;
			if (blocProperties.health <= 0) 
				Destroy(other.gameObject);
			Destroy(gameObject);
		}
		else if (other.CompareTag("Switch"))
		{
			other.GetComponent<Switch>().Activate();
			Destroy(gameObject);
		}
	}
}



