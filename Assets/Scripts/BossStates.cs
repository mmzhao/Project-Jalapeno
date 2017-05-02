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


public class Moving : BossStates
{
    // Maximum speed that boss can move.
    float maxSpeed;
    // Current speed that boss is moving.
    float curSpeed;

    public Moving(BossController mybc)
    {
        bc = mybc;
        player = GameObject.FindGameObjectWithTag("Player");
        maxSpeed = mybc.maxSpeed;
        curSpeed = 0;
    }
    public override void Enter() {; }
    public override void FixedUpdate() {; }
    public override void Update() {; }
    public override void Exit() {; }

    public void SearchForTarget(int mode) {; }

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

