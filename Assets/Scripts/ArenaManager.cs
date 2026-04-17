using UnityEngine;

public class ArenaManager : MonoBehaviour
{
	[Header("Camera & Walls")]
	public SimpleFollowCamera customCamera; 
	public Transform arenaCenter;      
	public GameObject leftWall;       
	public GameObject rightWall;      

	[Header("Combat Settings")]
	public GameObject[] waves;     
	private int currentWaveIndex = 0;
	private int enemyCount = 0;        
	private bool isLocked = false;
    
	private Transform playerTransform;

	void Start()
	{
		leftWall.SetActive(false);
		rightWall.SetActive(false);
        
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(player != null) playerTransform = player.transform;

		foreach(GameObject wave in waves) if(wave != null) wave.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !isLocked)
		{
			StartCombat();
		}
	}

	void StartCombat()
	{
		isLocked = true;

		if (customCamera != null && arenaCenter != null) 
		{
			customCamera.target = arenaCenter;
		}

		leftWall.SetActive(true);
		rightWall.SetActive(true);
		SpawnNextWave();
	}

	void SpawnNextWave()
	{
		if (currentWaveIndex < waves.Length)
		{
			waves[currentWaveIndex].SetActive(true);
			enemyCount = waves[currentWaveIndex].transform.childCount;

			// FIX: Tell every enemy in this wave that THIS specific arena is their manager.
			EnemyBase[] enemiesInWave = waves[currentWaveIndex].GetComponentsInChildren<EnemyBase>();
			foreach (EnemyBase enemy in enemiesInWave)
			{
				enemy.myManager = this;
			}
		}
		else { EndCombat(); }
	}

	public void EnemyDied()
	{
		enemyCount--;
		if (enemyCount <= 0)
		{
			currentWaveIndex++;
			if (currentWaveIndex < waves.Length) SpawnNextWave();
			else EndCombat();
		}
	}

	void EndCombat()
	{
		leftWall.SetActive(false);
		rightWall.SetActive(false);
        
		if (customCamera != null && playerTransform != null) 
		{
			customCamera.target = playerTransform;
		}
	}
}