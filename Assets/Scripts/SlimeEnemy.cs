using UnityEngine;

public class SlimeEnemy : EnemyBase // <--- 1. Changed MonoBehaviour to EnemyBase
{
	public enum SlimeState { Lunge, Shoot, Retreat, Idle }
	public SlimeState currentState = SlimeState.Idle;

	[Header("Slime Movement Stats")]
	// 2. Removed 'health' variable because EnemyBase already has it!
	public float lungeSpeed = 5f;
	public float retreatSpeed = 3f;
    
	[Header("Shooting")]
	public GameObject slimeProjectile;
	public Transform shootPoint;
	public float shootDelay = 1.5f;

	private Rigidbody2D rb;
	private Transform player;
	private float stateTimer;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
        
		// Added a quick safety check in case the player isn't in the scene yet
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null) 
		{
			player = playerObj.transform;
		}
        
		SetState(SlimeState.Lunge);
	}

	void Update()
	{
		// Safety check: if the player died, don't try to move towards them
		if (player == null) return; 

		stateTimer -= Time.deltaTime;

		switch (currentState)
		{
		case SlimeState.Lunge:
			// Move toward player
			Vector2 lungeDir = (player.position - transform.position).normalized;
			rb.velocity = lungeDir * lungeSpeed;
			if (stateTimer <= 0) SetState(SlimeState.Shoot);
			break;

		case SlimeState.Shoot:
			// Stop and shoot
			rb.velocity = Vector2.zero;
			if (stateTimer <= 0) {
				Shoot();
				SetState(SlimeState.Retreat);
			}
			break;

		case SlimeState.Retreat:
			// Move back to the right side of the screen
			rb.velocity = Vector2.right * retreatSpeed;
			if (stateTimer <= 0) SetState(SlimeState.Lunge);
			break;
		}
	}

	void SetState(SlimeState newState)
	{
		currentState = newState;
		// Different states last for different times
		if (newState == SlimeState.Lunge) stateTimer = 2f;
		if (newState == SlimeState.Shoot) stateTimer = 1f;
		if (newState == SlimeState.Retreat) stateTimer = 1.5f;
	}

	void Shoot()
	{
		if (slimeProjectile != null && shootPoint != null)
		{
			GameObject proj = Instantiate(slimeProjectile, shootPoint.position, Quaternion.identity);
			Vector2 shootDir = (player.position - transform.position).normalized;
            
			// Using '?.' is a safe way to call Setup in case the script is missing
			proj.GetComponent<EnemyProjectile>()?.Setup(shootDir);
		}
	}

	// 3. Removed TakeDamage() entirely! EnemyBase handles taking damage, dropping points, 
	// and telling the ArenaManager that it died.
}