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

    // SEARCHLIGHT MODE VARIABLES
    // Direction we are facing.
    Direction facing;
    // Length of time we face in any one direction.
    float facingLimit;
    // Length of time we have been facing in given direction.
    float facingTime;
    // Field of view in degrees from faced direction (ex. if you want to see a quarter of the map while facing NE, field of view = 45).
    float fieldOfView;

    public Targeting(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
        curSpeed = 0;
        playerOldPos = player.transform.position;
        playerCurrPos = player.transform.position;
        facingLimit = bc.facingLimit;
        facingTime = 0;
    }
    public override void Enter() {; }
    public override void FixedUpdate()
    {
        // Find current position of player (this becomes the old position of the player at the end of FixedUpdate() as well).
        playerCurrPos = player.transform.position;

        // Motion-Sensing Mode
        if (bc.mode == 0)
        {
            if (bc.detectTime >= bc.detectLimit)
            {
                bc.stateEnded = true;
                bc.nextState = new Transitioning(bc);
            }
            bc.detected = SearchForTarget(0);
            if (bc.detected)
            {
                Vector3 vecToPlayer = playerCurrPos - bc.rb.position;
                int walllayer = 9; // fill in with the layer # of anything that obstructs enemy vision
                if (!Physics.Raycast(bc.rb.position, vecToPlayer, vecToPlayer.magnitude, (1 << walllayer)))
                {
                    Move(bc.rb.position);
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
            bc.detected = SearchForTarget(1);
            if (bc.detected)
            {
                bc.stateEnded = true;
                bc.nextState = new Attacking(bc);
            }
            facingTime += Time.deltaTime;
            if (facingTime >= facingLimit)
            {
                facing = DirectionUtil.RightAdjacent(facing);
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
        else
        {
            return false;
        }
    }

    public void Move(Vector3 d) {; } 
}

public class Attacking : BossStates
{
    public Attacking(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void Enter() {; }
    public override void FixedUpdate() {; }
    public override void Update() {; }
    public override void Exit() {; }

    public void Attack(int mode) {; }
}

public class Transitioning : BossStates
{
    // numSpawn indicates how many new enemies to spawn when transitioning from Searchlight back to Motion-Sensing.
    public int numSpawn; 
    public Transitioning(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
        numSpawn = 0;
    }
    public override void Enter()
    {
        if (bc.mode == 1)
        {
            numSpawn = bc.initialSpawn + bc.minionScaling * bc.numResets;
            bc.SpawnEnemies(numSpawn);
        }
    }
    public override void FixedUpdate() {; }
    public override void Update() {; }
    public override void Exit() {; }
}

public class Dying : BossStates
{
    public Dying(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void Enter() {; }
    public override void FixedUpdate() {; }
    public override void Update() {; }
    public override void Exit() {; }
}

