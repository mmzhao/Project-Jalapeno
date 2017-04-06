using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : PlayerAttack {

    /** Fields from PlayerAttack:
     * PlayerController pc;
     * int counter;
     * int donecount;
     */
    new public readonly PlayerStateIndex playerState = PlayerStateIndex.IDLE_ATTACK_2;
    BoxCollider hitbox;
	Vector3 facing;

	// make a cube to show hitbox
	//	GameObject myCube;
	GameObject attack;

	public Attack2(PlayerController controller) : base(controller)
	{
		pc = controller;
		counter = 0;
		donecount = 24;
		facing = pc.playerToMouse;
		pc.attack2Charges -= 1;
	}

	// Create hitboxes, start animation
	public override void Enter()
	{
        this.pc.anim.SetInteger(animState, (int)playerState);
        pc.anim.SetFloat("p2mX", pc.playerToMouse.x);
        pc.anim.SetFloat("p2mZ", pc.playerToMouse.z);
        pc.anim.SetFloat("velocityX", pc.playerToMouse.x);
        pc.anim.SetFloat("velocityZ", pc.playerToMouse.z);

        attack = (GameObject) GameObject.Instantiate(pc.ap2);
		attack.transform.position = pc.transform.position + new Vector3(0, 2, 0);
		attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (facing.z, facing.x) * 180f / Mathf.PI, 0);
		foreach (Transform hitbox in attack.transform) 
		{
			hitbox.gameObject.SetActive (false);
		}
		//        hitbox = pc.gameObject.AddComponent<BoxCollider>();
		//        Debug.Log(facing);
		//        hitbox.center = (pc.rb.transform.position + 10 * DirectionUtil.DirToVector(facing));
		//        hitbox.size = (new Vector3(10, 1, 10));

		// make a sphere to show hitbox
		//        myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//        Renderer ren = myCube.GetComponents<Renderer>()[0];
		//		ren.material.color = Color.blue;
		//        myCube.transform.localScale = new Vector3(10, 1, 10);
		//        myCube.transform.position = hitbox.center;
	}

	public override void FixedUpdate()
	{
        base.FixedUpdate();
        //		Debug.Log (donecount + " " + counter);
        int hitboxIndex = 0;
		foreach (Transform hitbox in attack.transform) 
		{
			//				Debug.Log (curMoves + " " + numMoves + " " + curMoves / (numMoves / 5));
			if (counter / (donecount / 8) == hitboxIndex) {
				//					Debug.Log (hitboxIndex + " True");
				hitbox.gameObject.SetActive (true);
				break;
			} else {
				//					Debug.Log (hitboxIndex + " False");
				hitbox.gameObject.SetActive (false);
			}
			hitboxIndex++;
		}

		counter += 1;
		if (counter >= donecount)
		{
			//			pc.nextState = new PlayerMovement.Idle (pc);
			pc.stateEnded = true;
		}
	}

	public override void Update()
	{
		if (counter < donecount / 2)
			return;
		if (Input.GetButton ("Attack1") && pc.canAttack1 ()) 
		{
			pc.stateEnded = true;
			pc.nextState = new Attack1(pc);
		}
//		if (Input.GetButton ("Attack2") && pc.canAttack2 ()) 
//		{
//			pc.stateEnded = true;
//		}
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
