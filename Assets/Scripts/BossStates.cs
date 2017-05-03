using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStates {
    public BossController bc;
    public GameObject player;

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
}


public class Targeting : BossStates
{
    // MOTION-SENSING MODE VARIABLES
    // Current speed that boss is moving.
    float curSpeed;
    // Player old position and new position -> helps us find out if player has moved.
    Vector3 playerOldPos;
    Vector3 playerCurrPos;

    public Targeting(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
        curSpeed = bc.maxSpeedMotion;
        playerOldPos = player.transform.position;
        playerCurrPos = player.transform.position;
        bc.detectTime = 0;
        bc.facingTime = 0;
    }
    public override void Enter() {Debug.Log("Entering targeting for mode " + bc.mode); }
    public override void FixedUpdate()
    {
        // Find current position of player (this becomes the old position of the player at the end of FixedUpdate() as well).
        playerCurrPos = player.transform.position;

        // Motion-Sensing Mode
        if (bc.mode == 0)
        {
            bc.detected = SearchForTarget(0);
            if (bc.detectTime >= bc.detectLimit)
            {
                bc.stateEnded = true;
                bc.nextState = new Transitioning(bc);
            }
            if (bc.detected)
            {
                Vector3 vecToPlayer = playerCurrPos - bc.rb.position;
                int walllayer = 9; // fill in with the layer # of anything that obstructs enemy vision
                if (!Physics.Raycast(bc.rb.position, vecToPlayer, vecToPlayer.magnitude, (1 << walllayer)))
                {
                    Move(playerCurrPos);
                    bc.anim.SetFloat("moveX", vecToPlayer.x);
                    bc.anim.SetFloat("moveZ", vecToPlayer.z);
                }
                else
                {
                    bc.anim.SetFloat("moveX", 0);
                    bc.anim.SetFloat("moveZ", 0);
                }
            }
            else
            {
                bc.detectTime += Time.deltaTime;
            }
        }
        
        // Searchlight Mode
        if (bc.mode == 1)
        {
            Debug.Log("Searching for target (1)");
            bc.detected = SearchForTarget(1);
            if (bc.detected)
            {
                bc.stateEnded = true;
                bc.nextState = new Attacking(bc);
            }
            bc.facingTime += Time.deltaTime;
            if (bc.facingTime >= bc.facingLimit)
            {
                Debug.Log("Calling RightAdjacent");
                bc.facing = DirectionUtil.RightAdjacent(bc.facing);
                bc.facingTime = 0;
            }
        }

        // Current position becomes old position
        playerOldPos = playerCurrPos;
        
    }
    public override void Update() {; }
    public override void Exit() {; }

    public bool SearchForTarget(int mode)
    {
        // Motion-Sensing Mode
        if (mode == 0)
        {
            if (playerOldPos != playerCurrPos)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Seachlight Mode
        else
        {
            Vector3 vecToPlayer = playerCurrPos - bc.rb.position;
            float dp = Vector3.Dot(DirectionUtil.DirToVector(bc.facing), vecToPlayer.normalized);
            float angle = Mathf.Acos(dp);
            int walllayer = 9; // fill in with the layer # of anything that obstructs enemy vision
            
            if (angle <= bc.fieldOfView)
            {
                if (!Physics.Raycast(bc.rb.position, vecToPlayer, vecToPlayer.magnitude, (1 << walllayer)))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void Move(Vector3 d)
    {
        Vector3 dif = (d - bc.transform.position).normalized * curSpeed * Time.deltaTime;
        bc.rb.MovePosition(bc.transform.position + dif);
    } 
}

public class Attacking : BossStates
{
	GameObject[] hitboxes;
	float[] activateHitboxMoments = { 0f }; // these mark the timestamp to move to the next hitbox
	float[] deactivateHitboxMoments = { .5f};
	Vector3 facing;
	int numHitboxes;
	int activateHitboxIndex = 0;
	int deactivateHitboxIndex = 0;
	int damage;
	protected float attackDuration;
	protected float attackTimer;

	public Attacking(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
		facing = (player.transform.position - bc.gameObject.transform.position).normalized;
		facing.y = 0;
		attackDuration = 0.5f;
		attackTimer = 0;
		numHitboxes = activateHitboxMoments.Length;
		hitboxes = new GameObject[numHitboxes];
		damage = 30;
    }
    public override void Enter()
    {
        Attack(bc.mode);
    }
    public override void FixedUpdate()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDuration)
        {
            bc.stateEnded = true;
            bc.nextState = new Targeting(bc);
        }
    }
    public override void Update() {; }
    public override void Exit() {; }

    public void Attack(int mode)
    {
        if (mode == 0)
        {
            // Motion-Sensing Mode currently has no attack.
        }
        // INSERT ATTACK IN THE DIRECTION YOU ARE FACING.
        if (mode == 1)
        {
            // This is the angle difference between spikes
            float angularDiff = 3f;
            // Instantiate spike attacks 2 at a time
            for (int i = 0; i < 17; i += 1)
            {
				GameObject attack = (GameObject) GameObject.Instantiate(bc.projectile);
				damage = attack.GetComponent<AttackVariables>().Damage();
				attack.transform.parent = bc.transform;
				attack.transform.position = bc.transform.position + new Vector3(0, 2, 0) + facing.normalized*8;
				attack.transform.localEulerAngles = new Vector3 (90, 0, Mathf.Atan2 (facing.z, facing.x) * 180f / Mathf.PI-90+(i-8)*angularDiff);
				//			attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (facing.z, facing.x) * 180f / Mathf.PI, 0);
				//			Transform hitboxContainer = findHitboxesByTag(attack.transform);
				//			for (int i = 0; i < numHitboxes; i++)
				//			{
				//				hitboxes[i] = hitboxContainer.GetChild(i).gameObject;
				//				//				Debug.Log (hitboxes [i]);
				//				//				hitboxes[i].SetActive(false);
				//			}
				attack.GetComponent<ProjectileController>().dir = (Quaternion.AngleAxis((8-i)*angularDiff , Vector3.up) * facing).normalized;
			}
        }
    }
}

public class Transitioning : BossStates
{
    // numSpawn indicates how many new enemies to spawn when transitioning from Searchlight back to Motion-Sensing.
    public int numSpawn;
    // How long the transition animation should take.
    float transitionDuration;
    // How long the transition animation has taken.
    float transitionTime;
    // When we should trigger minion spawn.
    float transitionSpawn;
    bool hasSpawned;

    public Transitioning(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
        numSpawn = 0;
        transitionDuration = 0;
        transitionTime = 0;
        transitionSpawn = 0;
        hasSpawned = false;
    }
    public override void Enter() {
        Debug.Log("Transitioning from " + bc.mode + " to " + (1-bc.mode)); } // Start transition animation here? 

    public override void FixedUpdate()
    {
        transitionTime += Time.deltaTime;
        if (transitionTime >= transitionSpawn && !hasSpawned)
        {
            if (bc.mode == 1)
            {
                // Minion Spawning
                bc.numResets += 1;
                numSpawn = bc.initialSpawn + bc.minionScaling * bc.numResets;
                bc.SpawnEnemies(numSpawn);

                // Movespeed Increases
                bc.maxSpeedMotion += bc.speedScaling;
            }
            hasSpawned = true;
        }
        if (transitionTime >= transitionDuration)
        {
            bc.stateEnded = true;
            bc.nextState = new Targeting(bc);
            bc.mode = 1 - bc.mode;
        }
       
    }
    public override void Update() {; }
    public override void Exit() {; }
}

public class Dying : BossStates
{
    // Dying animation length
    float dieDuration;
    // Dying animation timer
    float dieTimer;

    public Dying(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void Enter()
    {
        Die();
    }
    public override void FixedUpdate()
    {
        dieTimer += Time.deltaTime;
        if (dieTimer >= dieDuration)
        {
            ; // Go to end screen
        }
    }
    public override void Update() {; }
    public override void Exit() {; }

    // Die animation here
    public void Die()
    {

        GameObject dt = (GameObject)GameObject.Instantiate(bc.dialogueTriggerZone, GameObject.FindGameObjectsWithTag("Player")[0].transform, true);
        GameObject.Destroy(bc.gameObject);
    }
}

