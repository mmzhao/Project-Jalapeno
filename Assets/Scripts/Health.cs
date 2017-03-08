using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
	public int startingHealth = 100;
	public int currentHealth;
//	public Image damageImage;
//	public float flashSpeed = 5f;
//	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

//	PlayerMovement playerMovement;                              // Reference to the player's movement.
//	PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
//	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.
	int numFlashes = 3;
	int curFlashes;

	void Awake ()
	{
//		playerMovement = GetComponent <PlayerMovement> ();
//		playerShooting = GetComponentInChildren <PlayerShooting> ();

		// Set the initial health of the player.
		currentHealth = startingHealth;
	}


	void Update ()
	{
		// If the player has just been damaged...
//		Debug.Log(damaged);
		if(damaged)
		{
			SpriteRenderer sr = gameObject.GetComponentsInChildren<SpriteRenderer> ()[0];
			sr.color = Color.red;
			curFlashes++;
			// ... set the colour of the damageImage to the flash colour.
//			damageImage.color = flashColour;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
//			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
			SpriteRenderer sr = gameObject.GetComponentsInChildren<SpriteRenderer> ()[0];
			sr.color = Color.white;
		}

		// Reset the damaged flag.
		if (curFlashes >= numFlashes) {
			damaged = false;
		}
	}


	public void TakeDamage (int amount)
	{
		curFlashes = 0;

		// Set the damaged flag so the screen will flash.
		damaged = true;

		// Reduce the current health by the damage amount.
		currentHealth -= amount;

		// If the player has lost all it's health and the death flag hasn't been set yet...
//		if(currentHealth <= 0 && !isDead)
//		{
			// ... it should die.
//			Death ();
//		}
	}
		
}