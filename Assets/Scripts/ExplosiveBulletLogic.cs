using UnityEngine;

public class ExplosiveBulletLogic : MonoBehaviour
{
	public GameObject explosionPrefab;
	public float speed = 12f; // Made it a bit faster for feel
	private Vector2 moveDirection = Vector2.right; // Default direction

	// This is called by the PlayerController
	public void Setup(Vector2 dir)
	{
		moveDirection = dir;
	}

	void Update()
	{
		// Move every frame
		transform.Translate(moveDirection * speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Check if we hit an Enemy or a Wall
		if (other.CompareTag("Enemy"))
		{
			Explode();
		}
	}

	void Explode()
	{
		if (explosionPrefab != null)
		{
			Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		}
        
		Destroy(gameObject); // Remove the bullet after it explodes
	}
}
