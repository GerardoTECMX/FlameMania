using UnityEngine;

public class PowerUp : MonoBehaviour
{
	// These match your PlayerController names
	public enum PowerUpType { Health, SpreadShot, ExplosiveShot, RapidShot }
	public PowerUpType type;
    
	[Header("Behavior Settings")]
	public float lifetime = 15f;      
	public float floatSpeed = 3f;     
	public float floatHeight = 0.25f; 

	private Vector3 startPos;

	private void Start()
	{
		startPos = transform.position;
		Destroy(gameObject, lifetime);
	}

	private void Update()
	{
		// Gentle bobbing in place
		float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
		transform.position = new Vector3(transform.position.x, newY, transform.position.z);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			ApplyPowerUp(other.gameObject);
			Destroy(gameObject);
		}
	}

	void ApplyPowerUp(GameObject player)
	{
		PlayerStats stats = player.GetComponent<PlayerStats>();
		PlayerController controller = player.GetComponent<PlayerController>();

		if (type == PowerUpType.Health)
		{
			if (stats != null)
			{
				stats.currentHealth = Mathf.Min(stats.currentHealth + 25f, stats.maxHealth);
				stats.UpdateUI();
			}
		}
		else
		{
			int amountToGive = 0;

			switch (type)
			{
			case PowerUpType.ExplosiveShot:
				controller.currentSecondary = PlayerController.SecondaryType.Explosive;
				amountToGive = 5; 
				break;
                
			case PowerUpType.SpreadShot: 
				controller.currentSecondary = PlayerController.SecondaryType.Spread;
				amountToGive = 30; 
				break;
                
			case PowerUpType.RapidShot: // Matches your "Rapid" enum
				controller.currentSecondary = PlayerController.SecondaryType.Rapid;
				amountToGive = 100; 
				break;
			}

			if (controller != null)
			{
				controller.secondaryAmmo = amountToGive;
			}
            
			if (stats != null)
			{
				stats.UpdateUI(); 
			}
		}
	}
}