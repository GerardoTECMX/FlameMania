using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
	public float maxHealth = 100f;
	public float currentHealth;
    
	public static int lives = 3;
	public static int score = 0; 

	[Header("UI References")]
	public TextMeshProUGUI livesText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI ammoText;
	public Slider healthBar;
	public GameObject gameOverPanel;

	private PlayerController playerController;

	void Start()
	{
		currentHealth = maxHealth;
		playerController = GetComponent<PlayerController>();

		if (healthBar != null)
		{
			healthBar.maxValue = maxHealth;
			healthBar.value = currentHealth; // Set initial value
		}
        
		if (gameOverPanel != null) gameOverPanel.SetActive(false);
        
		UpdateUI(); 
	}

	void Update()
	{
		UpdateAmmoUI();
	}

	public void AddPoints(int amount)
	{
		score += amount;
		UpdateUI();
	}

	public void TakeDamage(float amount)
	{
		currentHealth -= amount;
		UpdateUI(); // Trigger UI update immediately on damage

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	// This handles the instant update for health, score, and lives
	public void UpdateUI()
	{
		if (livesText != null) livesText.text = "Lives: " + lives;
		if (scoreText != null) scoreText.text = "Score: " + score;
        
		// FIX: Update the slider value whenever UI is refreshed
		if (healthBar != null)
		{
			healthBar.value = currentHealth;
		}
	}

	void UpdateAmmoUI()
	{
		if (ammoText != null && playerController != null)
		{
			if (playerController.currentSecondary == PlayerController.SecondaryType.None)
				ammoText.text = "Ammo: --";
			else
				ammoText.text = playerController.currentSecondary.ToString() + ": " + playerController.secondaryAmmo;
		}
	}

	public void Die()
	{
		lives--;
		if (lives > 0)
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else
		{
			if (gameOverPanel != null)
			{
				gameOverPanel.SetActive(true);
				Time.timeScale = 0f; 
			}
			else
			{
				PlayerStats.lives = 3;
				PlayerStats.score = 0;
				SceneManager.LoadScene("MainMenu");
			}
		}
	}

	public void RestartButton()
	{
		PlayerStats.lives = 3;
		PlayerStats.score = 0;
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}
}