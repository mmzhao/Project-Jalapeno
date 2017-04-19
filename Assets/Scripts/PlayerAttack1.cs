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
    float[] hitboxTransitionMarkers = { 0f, .15f, .23f, .35f, .45f };
	Vector3 facing;
    int numHitboxes;
    int currentHitboxIndex = 0;
    int damage;
    int thrust = 2000;
    bool alt = false;

    // make a cube to show hitbox
    //	GameObject myCube;
    GameObject attack;

    public Attack1(PlayerController controller) : base(controller)
    {
        pc = controller;
        counter = 0;
        donecount = 10;
		facing = pc.playerToMouse.normalized;
		pc.attack1Charges -= 1;
        attackDuration = 0.65f;
        numHitboxes = hitboxTransitionMarkers.Length;
        hitboxes = new GameObject[numHitboxes];
        damage = 0;
    }

    public Attack1(PlayerController controller, bool alt) : this(controller)
    {
        this.alt = alt;
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
        damage = attack.GetComponent<AttackVariables>().Damage();
        attack.transform.parent = pc.transform;
		attack.transform.position = pc.transform.position + new Vector3(0, 2, 0);
		attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (facing.z, facing.x) * 180f / Mathf.PI, 0);
        Transform hitboxContainer = findHitboxesByTag(attack.transform);
        for (int i = 0; i < numHitboxes; i++)
        {
            hitboxes[i] = hitboxContainer.GetChild(i).gameObject;
            
            hitboxes[i].SetActive(false);
        }

        // make CrystalGuy thrust forward
        pc.rb.velocity = Vector3.zero;
        Debug.Log(facing);
        pc.rb.AddForce(facing * thrust);

        // flip our hitboxes if attack is in alt mode
        if (alt) attack.transform.Rotate(new Vector3(180, 0, 0));
    }

    public override void FixedUpdate()
    {
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
        
    }

    public override void Update()
	{	
		if (currentHitboxIndex < numHitboxes / 2)
			return;
        if (currentHitboxIndex == numHitboxes - 1 && Input.GetButton("Attack1") && pc.canAttack1())
        {
            pc.stateEnded = true;
            pc.nextState = new Attack1(pc, !alt);
        }
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

    // Destroy hitboxes
    public override void Exit()
    {
		GameObject.Destroy(attack);
    }
   
}
