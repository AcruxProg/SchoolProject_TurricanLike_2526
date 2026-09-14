using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
	[Header("Specific Settings")]
	public float detectionRadius = 8.0f;
	public LayerMask obstructionLayer; 
	public float hoverAmplitude = .3f;
	public float hoverSpeed = 2.0f;
	public float fireRate = 1.0f;
	public float turretBulletSpeed = 6.0f;

	[Header("References")]
	public Transform firePoint;
	public TurretBullet turretBulletPrefab;
	public Rigidbody2D TurretRB;

	private Vector3 originPos;
	private Transform player;
	private float nextFire;
	

	
	void Start()
	{
		originPos = transform.position;
	}

	
	void FixedUpdate()
	{
		// if (!isAlive) return;
		// Vector3 hoverPos = originPos + Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
		// TurretRB.MovePosition(hoverPos);
	}

	void Update()
	{
		if (TurretRB == null)
		{
			Debug.Log("turretRB is null");
			return;
		}
		if (isAlive)
		{
			Vector3 hoverPos = originPos + Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
			TurretRB.MovePosition(hoverPos);
			
			
			if (player ==null && PlayerStatus.instance != null) 
				player= PlayerStatus.instance.transform;
			if (player== null)
				return;
			

			
			sprite.flipX = player.position.x> transform.position.x;
			
			bool playerInRange = false;
			float dist = Vector2.Distance(transform.position,player.position);

			if (dist <= detectionRadius)
			{
				
				RaycastHit2D hit = Physics2D.Linecast(firePoint.position, player.position,obstructionLayer);
				playerInRange = hit.collider ==null; 

				if (playerInRange && Time.time> nextFire)
				{
					nextFire = Time.time + 1.0f / fireRate;

					Vector2 dir = ((Vector2)player.position - (Vector2)firePoint.position).normalized;
					TurretBullet projectile =Instantiate(turretBulletPrefab,firePoint.position, Quaternion.identity);
					projectile.dir = dir;
					projectile.speed= turretBulletSpeed;
				}
			}
			
		}
		else
		{
			transform.GetComponent<Collider2D>().enabled = false; 
			transform.position += Vector3.down * Time.deltaTime;
			Destroy(gameObject, 1.0f);
		}
	}
}
