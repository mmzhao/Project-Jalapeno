using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielding : PlayerState
{
    new public readonly PlayerStateIndex playerState = PlayerStateIndex.SHIELD;
    float DECELERATION_CONSTANT = 40.0f;
    float STOP_THRESHOLD = 0.5f;
    public Collider playerShield;

    public Shielding (PlayerController pc) : base(pc)
    {
        this.playerShield = pc.playerShield;
    }

    public override void Enter()
    {
        pc.anim.SetInteger(animState, (int) playerState);
        pc.GetComponent<Collider>().enabled = false;
        playerShield.enabled = true;
    }

    public override void Exit()
    {
        pc.GetComponent<Collider>().enabled = true;
        playerShield.enabled = false;
    }

    public override void FixedUpdate()
    {
        if (pc.rb.velocity.magnitude > STOP_THRESHOLD)
        {
            pc.rb.velocity = Vector3.Lerp(pc.rb.velocity, Vector3.zero, DECELERATION_CONSTANT * Time.deltaTime);
        }
        else if (pc.rb.velocity.magnitude > 0)
        {
            pc.rb.velocity = Vector3.zero;
        }






    }

    public override void Update()
    {
        
    }
}
