using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	public float speed = 4f;
	public float damage = 10f;

	public void Setup(Vector2 direction)
	{
		GetComponent<Rigidbody2D>().velocity = direction * speed;
		Destroy(gameObject, 4f); // Self-destruct after 4 seconds
	}
	// Inside EnemyProjectile.cs
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerStats>()?.TakeDamage(damage);
			Destroy(gameObject);
		}
	}
}