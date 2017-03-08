using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : PlayerState {

    protected PlayerController pc;
    protected int counter;
    protected int donecount;

    // Create hitboxes, start animation
    public override abstract void Enter();

    public override abstract void FixedUpdate();
 
    public override abstract void Update();

    // Destroy hitboxes
    public override abstract void Exit();

    public override PlayerState HandleInput()
    {
        return new PlayerMovement.Idle(pc);
    }
}
