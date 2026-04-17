using UnityEngine;
using System.Collections; // Required for Coroutines

public class MediumEnemy : EnemyBase 
{
	[Header("Medium Stats")]
	public float fireRate = 3f;        // Time between bursts
	public float burstDelay = 0.2f;    // Time between shots in a burst
	public float idealDistance = 10f;  // Staying further away
	public float moveSpeed = 2.5f;
	public GameObject projectilePrefab;
    
	private float nextFire;
	private Transform player;
	private bool isFiring = false;

	void Start() 
	{ 
		player = GameObject.FindGameObjectWithTag("Player").transform; 
	}

	void Update() 
	{
		if (player == null) return;

		float dist = Vector2.Distance(transform.position, player.position);

		// Movement Logic: Kiting (Staying further away)
		if (dist > idealDistance + 1f) 
		{
			transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
		}
		else if (dist < idealDistance - 1f) 
		{
			transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
		}
        
		// Shooting Logic: Trigger Burst
		if (Time.time > nextFire && !isFiring) 
		{
			StartCoroutine(ShootBurst());
			nextFire = Time.time + fireRate;
		}
	}

	IEnumerator ShootBurst()
	{
		isFiring = true;

		for (int i = 0; i < 3; i++) // Shoot 3 times
		{
			if (player != null)
			{
				GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
				Vector2 dir = (player.position - transform.position).normalized;
				proj.GetComponent<EnemyProjectile>()?.Setup(dir);
			}
			yield return new WaitForSeconds(burstDelay);
		}

		isFiring = false;
	}
}