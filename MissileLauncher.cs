using UnityEngine;
using UnityEngine.InputSystem;


public class MissileLauncher : MonoBehaviour
{
	[Header("References")]
	public PlayerController playerController;
	public SmartMissileController missilePrefab;
	public Transform missileSpawnPoint;
	public InputActionAsset inputActions;

	[Header("Settings")]
	public float fireRatee = 1.0f;
	public float targetSearchRadius = 15.0f;

	private float nextFire;
	private PlayerStatus playerStatus;
	private InputAction fireMissileAction;

	
	private void Start()
	{
		playerStatus = playerController.GetComponent<PlayerStatus>();

		InputActionMap playerMap = inputActions.FindActionMap("Player");
		fireMissileAction = playerMap.FindAction("FireMissile");
		playerMap.Enable();
	}

	
	void Update()
	{
		float rightShoulder = fireMissileAction.ReadValue<float>();

		if (rightShoulder > .1f && playerStatus.isAbleToFireSmartMissiles && playerStatus.missiles > 0 && Time.time > nextFire)
		{
			nextFire = Time.time + 1.0f / fireRatee;
			
			float facing = playerController.spriteTop.flipX ? -1f : 1f;
			missileSpawnPoint.localPosition = new Vector3(Mathf.Abs(missileSpawnPoint.localPosition.x) * facing, missileSpawnPoint.localPosition.y, missileSpawnPoint.localPosition.z);

			SmartMissileController missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
			missile.target =FindNearestEnemy();
			missile.fallbackDir =new Vector2(facing, 0f);

			playerStatus.missiles--;
			playerStatus.UpdateStatusDisplay();
		}
	}

	
	private Transform FindNearestEnemy()
	{
		Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
		Transform nearest = null;
		float bestDist = targetSearchRadius;

		foreach (Enemy enemy in enemies)
		{
			if (!enemy.isAlive) continue;

			float dist = Vector2.Distance(transform.position, enemy.transform.position);
			if (dist < bestDist)
			{
				bestDist = dist;
				nearest = enemy.transform;
			}
		}
		return nearest;
	}
}
