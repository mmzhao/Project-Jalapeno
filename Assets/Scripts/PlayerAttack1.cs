using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : PlayerAttack {

    /** Fields from PlayerAttack:
     * PlayerController pc;
     * int counter;
     * int donecount;
     */
    new public readonly PlayerStateIndex playerState = PlayerStateIndex.IDLE_ATTACK;

    BoxCollider hitbox;
    GameObject[] hitboxes;
    float[] activateHitboxMoments = { 0f, .15f, .23f, .23f, .50f }; // these mark the timestamp to move to the next hitbox
    float[] deactivateHitboxMoments = { .15f, .23f, .5f, .5f, .65f };
	Vector3 facing;
    int numHitboxes;
    int activateHitboxIndex = 0;
    int deactivateHitboxIndex = 0;
    int damage;
    

    // make a cube to show hitbox
    //	GameObject myCube;
    GameObject attack;

    public Attack1(PlayerController controller) : base(controller)
    {
        pc = controller;
        counter = 0;
        donecount = 10;
		facing = pc.playerToMouse;
		pc.attack1Charges -= 1;
        attackDuration = 0.65f;
        numHitboxes = activateHitboxMoments.Length;
        hitboxes = new GameObject[numHitboxes];
        damage = 30;
        cancellableHitboxTime = .3f;
    }

    public override PlayerStateIndex getPlayerStateIndex()
    {
        return this.playerState;
    }

    // Create hitboxes, start animation
    public override void Enter()
    {
        this.pc.anim.SetInteger(animState, (int)playerState);
        pc.anim.SetFloat("p2mX", pc.playerToMouse.x);
        pc.anim.SetFloat("p2mZ", pc.playerToMouse.z);
        pc.anim.SetFloat("velocityX", pc.playerToMouse.x);
        pc.anim.SetFloat("velocityZ", pc.playerToMouse.z);

        attack = (GameObject) GameObject.Instantiate(pc.ap1);
        attack.transform.parent = pc.transform;
		attack.transform.position = pc.transform.position + new Vector3(0, 2, 0);
		attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (facing.z, facing.x) * 180f / Mathf.PI, 0);
        Transform hitboxContainer = findHitboxesByTag(attack.transform);
        for (int i = 0; i < numHitboxes; i++)
        {
            hitboxes[i] = hitboxContainer.GetChild(i).gameObject;
            hitboxes[i].SetActive(false);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        while (activateHitboxIndex < numHitboxes && attackTimer >= activateHitboxMoments[activateHitboxIndex])
        {
            hitboxes[activateHitboxIndex].SetActive(true);
            activateHitboxIndex++;
        }
        while (deactivateHitboxIndex < numHitboxes && attackTimer >= deactivateHitboxMoments[deactivateHitboxIndex])
        {
            hitboxes[deactivateHitboxIndex].SetActive(false);
            deactivateHitboxIndex++;
        }        
    }

    public override void Update()
	{	
		if (attackTimer < cancellableHitboxTime)
			return;
//		if (Input.GetButton ("Attack1") && pc.canAttack1 ()) 
//		{
//			pc.stateEnded = true;
//		}
		if (Input.GetButton ("Attack2") && pc.canAttack2 ()) 
		{
			pc.stateEnded = true;
			pc.nextState = new Attack2(pc);
		}
		if (Input.GetButton ("Dash") && pc.canDash () && (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))) 
		{
			pc.stateEnded = true;
			Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			pc.nextState = new PlayerMovement.Dash(pc, dir);
		}
        return;
    }

//	public override PlayerState HandleInput()
//	{
//		if (pc.stateEnded && ((Input.GetButton("Attack1") && pc.canAttack1()) || (Input.GetButton("Attack2")  && pc.canAttack2())))
//		{
//			if (Input.GetButton("Attack1") && pc.canAttack1())
//			{
//				return new Attack1(pc);
//			}
//			if (Input.GetButton("Attack2") && pc.canAttack2())
//			{
//				return new Attack2(pc);
//			}
//		}
//		if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
//		{
//			if (Input.GetButton("Dash") && pc.canDash())
//			{
//				Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
//				return new PlayerMovement.Dash(pc, dir);
//			}
//			return new PlayerMovement.Running(pc);
//		}
//		return new PlayerMovement.Idle(pc);
//	}

    // Destroy hitboxes
    public override void Exit()
    {
//        GameObject.Destroy(hitbox);

		// Destry the sphere
		// GameObject.Destroy(myCube);
		GameObject.Destroy(attack);
    }
   
}
