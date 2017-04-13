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
    float[] hitboxTransitionMarkers = { 0f, .2f, .4f, .6f, .8f };
	Vector3 facing;
    int numHitboxes;
    int currentHitboxIndex = 0;
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
        attackDuration = 1.0f;
        numHitboxes = hitboxTransitionMarkers.Length;
        hitboxes = new GameObject[numHitboxes];
        damage = 30;
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
        //		Debug.Log (Time.deltaTime);
        //		Debug.Log (donecount + " " + counter);
        base.FixedUpdate();
        if (currentHitboxIndex < numHitboxes && attackTimer >= hitboxTransitionMarkers[currentHitboxIndex])
        {
            if (currentHitboxIndex > 0)
            {
                hitboxes[currentHitboxIndex - 1].SetActive(false);
            }
            hitboxes[currentHitboxIndex].SetActive(true);
            currentHitboxIndex++;
        }
//        int hitboxIndex = 0;
//		foreach (Transform hitbox in attack.transform) 
//		{
//			//				Debug.Log (curMoves + " " + numMoves + " " + curMoves / (numMoves / 5));
//			if (counter / (donecount / 5) == hitboxIndex) {
//				//					Debug.Log (hitboxIndex + " True");
//				hitbox.gameObject.SetActive (true);
//				break;
//			} else {
//				//					Debug.Log (hitboxIndex + " False");
//				hitbox.gameObject.SetActive (false);
//			}
//			hitboxIndex++;
//		}

//        counter += 1;
//        if (counter >= donecount)
//        {
////			pc.nextState = new PlayerMovement.Idle (pc);
//            pc.stateEnded = true;
//        }
        
    }

    public override void Update()
	{	
		if (currentHitboxIndex < numHitboxes / 2)
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
