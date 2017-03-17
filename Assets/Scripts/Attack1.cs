using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : PlayerAttack {

    /** Fields from PlayerAttack:
     * PlayerController pc;
     * int counter;
     * int donecount;
     */
    new protected static readonly int playerState = 3;
    BoxCollider hitbox;
	Vector3 facing;

    // make a cube to show hitbox
//	GameObject myCube;
	GameObject attack;

    public Attack1(PlayerController controller) : base(controller)
    {
        pc = controller;
        counter = 0;
        donecount = 10;
		facing = pc.playerToMouse;
    }

    // Create hitboxes, start animation
    public override void Enter()
    {

		attack = (GameObject) GameObject.Instantiate(pc.ap1);
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
//		Debug.Log (donecount + " " + counter);
		int hitboxIndex = 0;
		foreach (Transform hitbox in attack.transform) 
		{
			//				Debug.Log (curMoves + " " + numMoves + " " + curMoves / (numMoves / 5));
			if (counter / (donecount / 5) == hitboxIndex) {
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
        if (counter == donecount)
        {
//			pc.nextState = new PlayerMovement.Idle (pc);
            pc.stateEnded = true;
        }
    }

    public override void Update()
    {
        return;
    }

	public override PlayerState HandleInput()
	{
		if (pc.stateEnded && Input.GetButton("Attack1"))
		{
			return new Attack1(pc);
		}
		if (pc.stateEnded && Input.GetButton("Attack2"))
		{
			return new Attack2(pc);
		}
		if (pc.stateEnded && Input.GetButton("Dash"))
		{
			return new PlayerMovement.Dash(pc);
		}
		if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Vertical")))
		{
			return new PlayerMovement.Running(pc);
		}
		if (pc.stateEnded)
		{
			return new PlayerMovement.Idle(pc);
		}
		return null;
	}

    // Destroy hitboxes
    public override void Exit()
    {
//        GameObject.Destroy(hitbox);

		// Destry the sphere
		// GameObject.Destroy(myCube);
		GameObject.Destroy(attack);
    }
}
