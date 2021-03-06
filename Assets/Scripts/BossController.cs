﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class BossController : MonoBehaviour {
    BossStates currentState;
    public BossStates nextState;
    public Rigidbody rb;
    public bool stateEnded { get; set; }
    public NavMeshAgent navAgent { get; set; }
    public Animator anim;
    
    // These variables manage movement.
    public float maxSpeedMotion; // Sets the maximum speed for Motion-Sensing mode.
    public float maxSpeedSearch; // Sets the maximum speed for Searchlight mode.

    // These variables manage resets between modes.
    public GameObject enemyPrefab; // Enemy Prefab.
    public int mode; // 0 = Motion-sensing; 1 = Searchlight. Keeps track of mode.
    public int numResets; // Keeps track of the number of times the boss has reset to motion-sensing mode.
    public int initialSpawn; // Sets how many minons spawn the very first time. 
    public int minionScaling; // Sets how many extra minions appear upon each reset.
    public int speedScaling; // Sets how much faster the boss can move upon each reset.

    // These variables manage detection.
    public float detectLimit; // Sets how long the player must be undetected for boss to switch to searchlight mode.
    public bool detected; // Indicates whether or not player has been detected.
    public float detectTime; // Keeps track of how long it has been since player was last detected (requires detected to be false).

    // These variables manage searchlight.
    public Direction facing; // Direction we are facing.
    public float facingLimit; // Length of time we face in any one direction.
    public float facingTime; // Length of time we have been facing in given direction.
    public float fieldOfView; // Field of view in degrees from faced direction (ex. if you want to see a quarter of the map while facing NE, field of view = 45).
	public GameObject attackObject; // The object with which we attack.
	public GameObject projectile;
    int hits;
    public int transitionLimit;

    // dialogue stuff
    public CutsceneDialogue iSeeYouCutsceneDialogue;
    public CutsceneDialogue deathCutsceneDialogue;
    public bool iSeeYouCutsceneDialogueHasPlayed { get; set; }

    void Awake()
    {
        // Initialize movement variables.
        maxSpeedMotion = 15;
        maxSpeedSearch = 20;
        // Initialize reset variables.
        mode = 0;
        numResets = 0;
        initialSpawn = 4;
        minionScaling = 2;
        speedScaling = 10;
        // Initialize detection variables.
        detectLimit = 5;
        detected = false;
        detectTime = 0;
        // Initialize searchlight variables.
        facing = Direction.S;
        facingLimit = 1.5f;
        facingTime = 0;
        fieldOfView = 0.79f;
        hits = 0;
        transitionLimit = 20;


        GameObject rootParent = this.transform.gameObject;
        if (rb == null)
        {
            rb = rootParent.GetComponent<Rigidbody>();
        }
        if (navAgent == null)
        {
            navAgent = rootParent.GetComponent<NavMeshAgent>();
        }
        if (anim == null)
        {
            anim = rootParent.GetComponent<Animator>();
        }
    }

    void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        currentState = new Targeting(this);
        SpawnEnemies(initialSpawn);
    }

    void Update() {; }
    void FixedUpdate()
    {
		for (int i = 0; i <= 8; i++) {
			gameObject.transform.GetChild (i).gameObject.SetActive (false);
		}
		if (mode == 1) {
			if (facing == Direction.S) {
				gameObject.transform.GetChild (1).gameObject.SetActive (true);
			} else if (facing == Direction.SW) {
				gameObject.transform.GetChild (2).gameObject.SetActive (true);
			} else if (facing == Direction.W) {
				gameObject.transform.GetChild (3).gameObject.SetActive (true);
			} else if (facing == Direction.NW) {
				gameObject.transform.GetChild (4).gameObject.SetActive (true);
			} else if (facing == Direction.N) {
				gameObject.transform.GetChild (5).gameObject.SetActive (true);
			} else if (facing == Direction.NE) {
				gameObject.transform.GetChild (6).gameObject.SetActive (true);
			} else if (facing == Direction.E) {
				gameObject.transform.GetChild (7).gameObject.SetActive (true);
			} else if (facing == Direction.SE) {
				gameObject.transform.GetChild (8).gameObject.SetActive (true);
			} 
		} else {
			gameObject.transform.GetChild (0).gameObject.SetActive (true);
		}
//		Debug.Log (currentState);
        rb.velocity = Vector3.zero;
        currentState.FixedUpdate();
        if (nextState != null)
        {
            stateEnded = false;
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            currentState.Enter();
        }
        if (this.gameObject.GetComponent<Health>().currentHealth <= 0)
        {
            nextState = new Dying(this);
        }
    }

    public void getHit(GameObject go, Collider other)
    {
        if (mode == 1)
        {
            if (other.gameObject.transform.root.tag == "Player")
            {
                Health h = GetComponent<Health>();
                Transform t = other.transform;
                while (t.parent != t.root) t = t.parent;

                AttackVariables av = t.GetComponent<AttackVariables>();
                int dmg = av.Damage();
                bool hit = h.TakeDamage(dmg, t);
                if (hit)
                {
                    av.audioSFX.playRandomOnHitClip(); // handle sounds
                    PlayerController pc = t.root.gameObject.GetComponent<PlayerController>();
                    if (pc != null) pc.addRage(av.rageGain);
                }
                Debug.Log("Get Hit");
                hits += 1;
                if (hits == transitionLimit)
                {
                    hits = 0;
                    stateEnded = true;
                    nextState = new Transitioning(this);
                }


            }
        }
    }

    // Spawns numSpawn number of enemies near the boss.
    public void SpawnEnemies(int numSpawn)
    {
        for (int i = 0 ; i < numSpawn / 2; i++)
        {
            GameObject enemy = (GameObject)GameObject.Instantiate(enemyPrefab);
            enemy.transform.position = rb.transform.position + new Vector3(20*i, 0, 0);
            GameObject enemy2 = (GameObject)GameObject.Instantiate(enemyPrefab);
            enemy2.transform.position = rb.transform.position + new Vector3(-20 * i, 0, 0);
        }
    }
}
