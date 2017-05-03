using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
	public int startingHealth = 100;
	public int currentHealth;
//	public Image damageImage;
//	public float flashSpeed = 5f;
//	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

//	PlayerMovement playerMovement;                              // Reference to the player's movement.
//	PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
//	bool isDead;                                                // Whether the player is dead.
	public bool damaged;                                               // True when the player gets damaged.
	int numFlashes = 20;
	int curFlashes;
    HashSet<Transform> hitBy;

	void Awake ()
	{
//		playerMovement = GetComponent <PlayerMovement> ();
//		playerShooting = GetComponentInChildren <PlayerShooting> ();

		// Set the initial health of the player.
		currentHealth = startingHealth;
        hitBy = new HashSet<Transform>();
	}


	void Update ()
	{
		// If the player has just been damaged...
//		Debug.Log(damaged);
		if(damaged)
		{
			SpriteRenderer sr = gameObject.GetComponentsInChildren<SpriteRenderer> ()[0];
			sr.color = Color.red;
			// ... set the colour of the damageImage to the flash colour.
//			damageImage.color = flashColour;
		}
        // Otherwise...
        if (curFlashes >= numFlashes)
        {
			// ... transition the colour back to clear.
//			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
			SpriteRenderer sr = gameObject.GetComponentsInChildren<SpriteRenderer> ()[0];
			sr.color = new Color (1f, 1f, 1f, sr.color.a);
            damaged = false;
		}

        curFlashes++;
    }

    public bool TakeDamage (int amount, Transform attack)
    {
        if (!hitBy.Contains(attack))
        {
            TakeDamage(amount);
            hitBy.Add(attack);
            return true;
        }
        return false;
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