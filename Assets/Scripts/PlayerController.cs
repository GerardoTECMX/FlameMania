using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Movement Variables")]
	public float HORIZONTALSPD = 4f;
	public float VERTICALSPD = 7f;

	[Header("Shooting General")]
	public GameObject BasicBullet;
	public Transform BulletSpawn;
	public float fireRate = 0.25f; 
	private float nextFireTime = 0f; 

	public enum SecondaryType { None, Spread, Rapid, Explosive }

	[Header("Power-ups (K Key)")]
	public SecondaryType currentSecondary = SecondaryType.None;
	public int secondaryAmmo = 0;
	public GameObject SpreadBulletPrefab;
	public GameObject ExplosiveBulletPrefab;
	public GameObject RapidBulletPrefab; // Assigned in Inspector
	public float rapidFireRate = 0.1f;    // Speed for automatic fire
	private float nextSecondaryFireTime = 0f; 

	[Header("Nuke (L Key)")]
	public GameObject NukePrefab; 

	private Rigidbody2D rb;
	private Vector2 MOVEMENTINPUT;
	private PlayerStats stats; 

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		stats = GetComponent<PlayerStats>();
		if (stats == null) Debug.LogWarning("PlayerStats script missing!");
	}

	void Update()
	{
		// 1. INPUTS
		bool isHoldingBasic = Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.Mouse1);
		bool isHoldingSecondary = Input.GetKey(KeyCode.K);
		bool pressedNuke = Input.GetKeyDown(KeyCode.L);

		// 2. MOVEMENT LOCK LOGIC
		// Halt movement if firing Basic (J) OR firing a "heavy" Secondary (Spread/Explosive)
		// Mobility is ONLY allowed when firing Rapid or when not shooting at all.
		bool shouldHalt = (isHoldingBasic && currentSecondary != SecondaryType.Rapid) || 
		(isHoldingSecondary && currentSecondary != SecondaryType.None && currentSecondary != SecondaryType.Rapid);

		if (shouldHalt)
		{
			MOVEMENTINPUT = Vector2.zero;
		}
		else
		{
			MOVEMENTINPUT.x = Input.GetAxisRaw("Horizontal");
			MOVEMENTINPUT.y = Input.GetAxisRaw("Vertical");
		}

		// 3. SHOOTING LOGIC - BASIC
		if (isHoldingBasic && Time.time >= nextFireTime)
		{
			SHOOT_BASIC();
			nextFireTime = Time.time + fireRate;
		}

		// 4. SHOOTING LOGIC - SECONDARY
		if (secondaryAmmo > 0 && isHoldingSecondary)
		{
			if (currentSecondary == SecondaryType.Rapid)
			{
				if (Time.time >= nextSecondaryFireTime)
				{
					SHOOT_SECONDARY();
					nextSecondaryFireTime = Time.time + rapidFireRate;
				}
			}
			else if (Input.GetKeyDown(KeyCode.K)) // One shot per press for Spread/Explosive
			{
				SHOOT_SECONDARY();
			}
		}

		if (pressedNuke) USE_NUKE();
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector2(MOVEMENTINPUT.x * HORIZONTALSPD, MOVEMENTINPUT.y * VERTICALSPD);
	}

	void SHOOT_BASIC()
	{
		if (BasicBullet == null) return;
		FireBullet(BasicBullet, Vector2.right);
	}

	void SHOOT_SECONDARY()
	{
		switch (currentSecondary)
		{
		case SecondaryType.Spread:
			FireBullet(SpreadBulletPrefab, new Vector2(1, 0.3f));
			FireBullet(SpreadBulletPrefab, Vector2.right);
			FireBullet(SpreadBulletPrefab, new Vector2(1, -0.3f));
			break;
		case SecondaryType.Explosive:
			FireBullet(ExplosiveBulletPrefab, Vector2.right);
			break;
		case SecondaryType.Rapid:
			FireBullet(RapidBulletPrefab, Vector2.right); // Uses Rapid prefab
			break;
		}

		secondaryAmmo--;
		if (stats != null) stats.UpdateUI();
		if (secondaryAmmo <= 0) currentSecondary = SecondaryType.None;
	}

	void USE_NUKE()
	{
		if (PlayerStats.lives > 0)
		{
			PlayerStats.lives--; 
			stats.UpdateUI();
			Instantiate(NukePrefab, transform.position, Quaternion.identity);
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				enemy.GetComponent<EnemyBase>()?.TakeDamage(9999);
			}
		}
	}

	void FireBullet(GameObject prefab, Vector2 dir)
	{
		if (prefab == null) return;
		GameObject b = Instantiate(prefab, BulletSpawn.position, Quaternion.identity);
		b.GetComponent<Bullet>()?.Setup(dir);
	}
}