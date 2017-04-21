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

        playerShield.SetActive(true);
        defaultHurtBox.SetActive(false);
    }

    public override void Exit()
    {
        playerShield.SetActive(false);
        defaultHurtBox.SetActive(true);
    }

    public override void FixedUpdate()
    {
        Debug.Log(pc.shieldTime);
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