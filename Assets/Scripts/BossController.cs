using System.Collections;
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

    void Awake()
    {
        // Initialize movement variables.
        maxSpeedMotion = 35;
        maxSpeedSearch = 35;
        // Initialize reset variables.
        mode = 0;
        numResets = 0;
        initialSpawn = 4;
        minionScaling = 2;
        speedScaling = 5;
        // Initialize detection variables.
        detectLimit = 2;
        detected = false;
        detectTime = 0;
        // Initialize searchlight variables.
        facing = Direction.S;
        facingLimit = 2;
        facingTime = 0;
        fieldOfView = 11.25f;


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
            currentState.Exit();
            nextState = null;
            currentState = new Dying(this);
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

                int dmg = t.GetComponent<AttackVariables>().Damage();
                h.TakeDamage(dmg, t);
            }
        }
    }

    // Spawns numSpawn number of enemies near the boss.
    public void SpawnEnemies(int numSpawn) {; }
}
