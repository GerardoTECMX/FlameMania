using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
	public float damage = 50f;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			// UPDATED: Get EnemyBase so it works for Slimes, Mediums, and Heavies
			EnemyBase enemy = other.GetComponent<EnemyBase>();
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
			}
		}
	}
}