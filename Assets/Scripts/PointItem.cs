using UnityEngine;

public class PointItem : MonoBehaviour
{
	[Header("Decay Settings")]
	public int maxPoints = 100;
	public int minPoints = 10;
	public float lifetime = 5f;

	[Header("Vacuum Settings")]
	public float collectionDist = 2f;
	public float vacuumSpeed = 10f;
    
	private Transform player;
	private bool isHoming = false;
	private float spawnTime;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		spawnTime = Time.time;
		// Self-destruct if not collected
		Destroy(gameObject, lifetime);
	}

	void Update()
	{
		if (player == null) return;
		float dist = Vector2.Distance(transform.position, player.position);

		// Touhou Vacuum Logic: Top of screen OR close to player
		float screenThresholdY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.75f, 0)).y;
        
		if (player.position.y > screenThresholdY || dist < collectionDist)
		{
			isHoming = true;
		}

		if (isHoming)
		{
			transform.position = Vector3.MoveTowards(transform.position, player.position, vacuumSpeed * Time.deltaTime);
		}

		if (dist < 0.5f)
		{
			Collect();
		}
	}

	void Collect()
	{
		float timeAlive = Time.time - spawnTime;
		float percentage = Mathf.Clamp01(timeAlive / lifetime);
        
		// Calculate decaying value
		int finalValue = Mathf.RoundToInt(Mathf.Lerp(maxPoints, minPoints, percentage));

		PlayerStats stats = player.GetComponent<PlayerStats>();
		if (stats != null) stats.AddPoints(finalValue);

		Destroy(gameObject);
	}
}
