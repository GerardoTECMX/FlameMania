using UnityEngine;

public class HeavyEnemy : EnemyBase 
{
	public enum HeavyState { LockOn, Dash, Retreat }
    
	[Header("State Machine")]
	public HeavyState currentState = HeavyState.LockOn;

	[Header("Movement Settings")]
	public float lockDuration = 2.0f;       // Time spent following player's Y
	public float dashSpeed = 18.0f;         // Speed of the forward lunge
	public float retreatSpeed = 5.0f;       // Speed of returning to the right
	public float verticalTrackingSpeed = 6.0f;
    
	[Header("Combat Settings")]
	public float contactDamage = 30f;       // Damage dealt to player

	private float timer;
	private Transform player;
	private Vector2 dashDirection;
	private float startX;                   // The "home" position on the right

	void Start()
	{
		// 1. Find the Player
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null) player = playerObj.transform;
        
		// 2. Initialize timer and starting X position
		timer = lockDuration;
		startX = transform.position.x; 
	}

	void Update()
	{
		if (player == null) return;

		switch (currentState)
		{
		case HeavyState.LockOn:
			HandleLockOn();
			break;

		case HeavyState.Dash:
			HandleDash();
			break;

		case HeavyState.Retreat:
			HandleRetreat();
			break;
		}
	}

	// PHASE 1: Follow the player's vertical movement
	private void HandleLockOn()
	{
		float targetY = Mathf.MoveTowards(transform.position.y, player.position.y, verticalTrackingSpeed * Time.deltaTime);
		transform.position = new Vector2(transform.position.x, targetY);

		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			// Lock in the direction for the dash
			dashDirection = (player.position - transform.position).normalized;
			currentState = HeavyState.Dash;
			timer = 0.8f; // How long the lunge lasts
		}
	}

	// PHASE 2: Lunge forward
	private void HandleDash()
	{
		transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
        
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			currentState = HeavyState.Retreat;
		}
	}

	// PHASE 3: Move back to the right side of the screen
	private void HandleRetreat()
	{
		// Use MoveTowards so he stops exactly at startX
		float targetX = Mathf.MoveTowards(transform.position.x, startX, retreatSpeed * Time.deltaTime);
		transform.position = new Vector2(targetX, transform.position.y);

		// Once back "home," start the cycle over
		if (Mathf.Abs(transform.position.x - startX) < 0.1f)
		{
			timer = lockDuration;
			currentState = HeavyState.LockOn;
		}
	}

	// --- DAMAGE DETECTION ---

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			ApplyDamageToPlayer(other.gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			ApplyDamageToPlayer(collision.gameObject);
		}
	}

	private void ApplyDamageToPlayer(GameObject playerObj)
	{
		PlayerStats stats = playerObj.GetComponent<PlayerStats>();
		if (stats != null)
		{
			stats.TakeDamage(contactDamage);
			Debug.Log("Heavy Unit hit the player for " + contactDamage + " damage!");
		}
	}
}