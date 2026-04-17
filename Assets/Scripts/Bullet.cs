using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float SPD = 12f;
	public float HALFLIFE = 2f;
	public float damage = 10f; // Added damage stat
    
	private Rigidbody2D rb;
    
	public void Setup(Vector2 direction)
	{
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = direction * SPD;
		Destroy(gameObject, HALFLIFE);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			// Look for EnemyBase (this covers Slimes, Mediums, and Heavies)
			EnemyBase enemy = other.GetComponent<EnemyBase>();
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
			}
			Destroy(gameObject); // Bullet disappears on hit
		}
	}
}
