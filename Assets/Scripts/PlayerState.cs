using System;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController pc;
    public readonly PlayerStateIndex playerState;
    protected static readonly string animState = "State";

    public PlayerState(PlayerController pc)
    {
        this.pc = pc;
    }

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
    public PlayerState HandleInput()
	{
		if (pc.stateEnded && ((Input.GetButton("Attack1") && pc.canAttack1()) || (Input.GetButton("Attack2")  && pc.canAttack2())))
		{
			if (Input.GetButton("Attack1") && pc.canAttack1())
			{
				return new Attack1(pc);
			}
			if (Input.GetButton("Attack2") && pc.canAttack2())
			{
				return new Attack2(pc);
			}
		}
		if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
		{
			if (Input.GetButton("Dash") && pc.canDash())
			{
				Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
				return new PlayerMovement.Dash(pc, dir);
			}
			return new PlayerMovement.Running(pc);
		}
		return new PlayerMovement.Idle(pc);
	}
    public virtual void Animate()
    {
        //pc.anim.SetFloat("p2mX", pc.playerToMouse.x); //player-to-mouse-X
        //pc.anim.SetFloat("p2mZ", pc.playerToMouse.z); //player-to-mouse-Z
    }
}

public enum PlayerStateIndex
{
    IDLE, RUN, DASH, JUMP, IDLE_ATTACK, SHIELD, IDLE_ATTACK_2
}