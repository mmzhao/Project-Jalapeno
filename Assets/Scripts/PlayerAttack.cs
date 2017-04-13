using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : PlayerState {

    protected int counter;
    protected int donecount;
     
    public PlayerAttack(PlayerController pc) : base(pc)
    {

    }


    // Destroy hitboxes
    public override abstract void Exit();
    
//    public override PlayerState HandleInput()
//    {
//        return new PlayerMovement.Idle(pc);
//    }
}
