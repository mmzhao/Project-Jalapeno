using System;
using UnityEngine;

public class Shielding : PlayerState
{
    new public readonly PlayerStateIndex playerState = PlayerStateIndex.SHIELD;
    public GameObject playerShield;
    public GameObject defaultHurtBox;

    public Shielding (PlayerController pc) : base(pc)
    {
        this.playerShield = pc.hurtBoxes.transform.Find("Shield").gameObject;
        this.defaultHurtBox = pc.hurtBoxes.transform.Find("Default").gameObject;
    }

    public override void Enter()
    {
        // handle all animations
        pc.anim.SetInteger(animState, (int) playerState);
        pc.anim.SetFloat("p2mX", pc.playerToMouse.x);
        pc.anim.SetFloat("p2mZ", pc.playerToMouse.z);
        pc.anim.SetFloat("velocityX", pc.playerToMouse.x);
        pc.anim.SetFloat("velocityZ", pc.playerToMouse.z);
        defaultHurtBox.SetActive(false);
        playerShield.SetActive(true);
    }

    public override void Exit()
    {
        playerShield.SetActive(false);
        defaultHurtBox.SetActive(true);
    }

    public override void FixedUpdate()
    {
        if (!playerShield.activeSelf) playerShield.SetActive(true);
        if (!Input.GetButton("Shield") || pc.shieldTime <= 0)
        {
			if (pc.shieldTime <= 0) {
                Debug.Log("shield broken!");
				pc.shieldBroken = true;
			}
            pc.stateEnded = true;
            return;
        }

        base.FixedUpdate();
        pc.shieldTime -= Time.deltaTime;

    }

    public new PlayerState HandleInput()
    {
        if (pc.stateEnded && ((Input.GetButton("Attack1") && pc.canAttack1()) || (Input.GetButton("Attack2") && pc.canAttack2())))
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
        }
        if (pc.movementInput == Vector3.zero)
        {
            return new PlayerMovement.Idle(pc);
        }
        return null;
    }

    public override void Update()
    {
        
    }

    public override void Animate()
    {
        pc.anim.SetFloat("p2mX", pc.playerToMouse.x);
        pc.anim.SetFloat("p2mZ", pc.playerToMouse.z);
    }

    public override PlayerStateIndex getPlayerStateIndex()
    {
        return this.playerState;
    }
}