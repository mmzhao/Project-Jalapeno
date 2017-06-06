using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public PlayerController pc;

    // Health variables
    public Health healthScript;
    public Slider healthBar;
    public Text healthText;

    // Shield variables
    public Slider shieldBar;
    public Text shieldText;

    // Rage variables
    public Slider rageBar;
    public Text rageText;


    // Use this for initialization
    void Start () {
        healthBar.minValue = 0;
        UpdateHealth();
        UpdateShield();

    }

    // Update is called once per frame
    void Update () {
        UpdateHealth();
        UpdateShield();
        UpdateRage();
    }

    void UpdateHealth ()
    {
        int maxHealth = healthScript.maxHealth;
        int currentHealth = healthScript.currentHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        healthText.text = currentHealth + "/" + maxHealth;
    }

    void UpdateShield ()
    {
        int maxShield = (int) pc.maxShieldTime;
        int currentShield = (int) pc.shieldTime;

        shieldBar.maxValue = maxShield;
        shieldBar.value = currentShield;

        shieldText.text = currentShield + "/" + maxShield;
    }

    void UpdateRage()
    {
        int maxRage = (int)pc.maxRage;
        int currentRage = (int)pc.currentRage;

        rageBar.maxValue = maxRage;
        rageBar.value = currentRage;

        rageText.text = currentRage + "/" + maxRage;
    }
}
