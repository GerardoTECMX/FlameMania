using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public void StartGame()
	{
		// Reset static variables so a new game starts fresh
		PlayerStats.lives = 3;
		PlayerStats.score = 0;
        
		// Ensure time is moving (in case you came from a Game Over screen)
		Time.timeScale = 1f;

		// Load the first level (make sure it's index 1 in Build Settings)
		SceneManager.LoadScene(1); 
	}

	public void QuitGame()
	{
		Debug.Log("Quit Pressed!");
		Application.Quit(); // Only works in the actual build, not the editor
	}
}