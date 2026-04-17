using UnityEngine;

// This 'inherits' from Bullet, so it gets the Setup() and hitting logic for free!
public class RapidBullet : Bullet
{
	private void Awake()
	{
		// Based on your original Bullet stats:
		// 1. Speed is increased from 12 to 20
		SPD = 20f; 

		// 2. Damage is cut in half from 10 to 5
		damage = 5f; 
        
		// 3. Optional: Reduce life span since it's faster
		HALFLIFE = 1.5f; 
	}
}
