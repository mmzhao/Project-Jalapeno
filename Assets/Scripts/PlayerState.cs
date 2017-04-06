using System;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController pc;
    public readonly PlayerStateIndex playerState;
    protected static readonly string animState = "State";

    //movement parameters
    float DECELERATION_CONSTANT = 20.0f;
    float STOP_THRESHOLD = 0.5f;

    public PlayerState(PlayerController pc)
    {
        this.pc = pc;
    }

    public abstract void Enter();
    public virtual void FixedUpdate()
    {
        DecelerateToStop();
    }
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
		else if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
		{
			if (Input.GetButton("Dash") && pc.canDash())
			{
				Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
				return new PlayerMovement.Dash(pc, dir);
			}
			return new PlayerMovement.Running(pc);
		}
        else if (pc.stateEnded && Input.GetButton("Shield") && pc.shieldTime >= 1)
        {
            return new Shielding(pc);
        }
		return new PlayerMovement.Idle(pc);
	}
    public virtual void Animate()
    {
        //pc.anim.SetFloat("p2mX", pc.playerToMouse.x); //player-to-mouse-X
        //pc.anim.SetFloat("p2mZ", pc.playerToMouse.z); //player-to-mouse-Z
    }

    private void DecelerateToStop()
    {
        if (pc.rb.velocity.magnitude > STOP_THRESHOLD)
        {
            pc.rb.velocity = Vector3.Lerp(pc.rb.velocity, Vector3.zero, DECELERATION_CONSTANT * Time.deltaTime);
        }
        else
        {
            pc.rb.velocity = Vector3.zero;
        }
    }
}

public enum PlayerStateIndex
{
    IDLE, RUN, DASH, JUMP, IDLE_ATTACK, SHIELD, IDLE_ATTACK_2
}