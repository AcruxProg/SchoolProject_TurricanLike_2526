using UnityEngine;


public class TurretBullet : MonoBehaviour
{
	public float speed = 6.0f;
	public int damage = 15;
	public Rigidbody2D turretBulletRB;
	public float lifeTime = 5.0f;
	

	public Vector2 dir;


	void Start()
	{
		Destroy(gameObject,lifeTime);
	}
	void Update()
	{
		if(turretBulletRB==null)
			return;
		
		turretBulletRB.linearVelocity = dir * speed;
	}
	
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy")) 
			return; 

		if (other.CompareTag("Player"))
		{
			Debug.Log("Turret bullet damage");
			PlayerStatus.instance.ReceiveDamage(damage);
			Destroy(gameObject);
		}
		
	}

	
	
}
