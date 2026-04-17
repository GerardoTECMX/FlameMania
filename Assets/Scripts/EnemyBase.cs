using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
	[Header("Base Stats")]
	public float health = 20f;
	public int pointValue = 100;
	public GameObject pointPrefab; 

	[Header("Drops")]
	public GameObject[] powerUpPrefabs; 
	[Range(0, 100)]
	public float dropChance = 20f; 

	[HideInInspector] public ArenaManager myManager; 
	
	// FIX: The lock that prevents double-counting!
	private bool isDead = false; 

	public virtual void TakeDamage(float damage)
	{
		// If we are already dead, ignore any extra damage this frame
		if (isDead) return; 

		health -= damage;
		if (health <= 0) 
		{
			isDead = true; // Lock it down!
			Die();
		}
	}

	protected virtual void Die()
	{
		if (pointPrefab != null) 
		{
			Instantiate(pointPrefab, transform.position, Quaternion.identity);
		}

		if (powerUpPrefabs.Length > 0 && Random.Range(0f, 100f) <= dropChance)
		{
			int randomIndex = Random.Range(0, powerUpPrefabs.Length);
			if (powerUpPrefabs[randomIndex] != null)
			{
				Instantiate(powerUpPrefabs[randomIndex], transform.position, Quaternion.identity);
			}
		}

		if (myManager != null) 
		{
			myManager.EnemyDied();
		}

		Destroy(gameObject);
	}
}
