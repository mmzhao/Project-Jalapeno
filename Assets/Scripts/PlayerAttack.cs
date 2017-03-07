using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerState {

    PlayerController pc;
    GameObject hitboxes;
    Collider[] colliders;

	PlayerAttack(PlayerController playerController)
    {
        this.pc = playerController;
        hitboxes = pc.gameObject.transform.Find("Hitboxes").gameObject;
        colliders = hitboxes.GetComponents<Collider>();
    }

    public override void Enter()
    {
        
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }

	public override PlayerState HandleInput()
    {
        return null;
    }
}
