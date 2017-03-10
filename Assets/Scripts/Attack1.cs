using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : PlayerAttack {

    /** Fields from PlayerAttack:
     * PlayerController pc;
     * int counter;
     * int donecount;
     */
    BoxCollider hitbox;
    Direction facing;

    // make a cube to show hitbox
    GameObject myCube;

    public Attack1(PlayerController controller)
    {
        pc = controller;
        counter = 0;
        donecount = 10;
        facing = pc.facing;
    }

    // Create hitboxes, start animation
    public override void Enter()
    {
        hitbox = pc.gameObject.AddComponent<BoxCollider>();
        Debug.Log(facing);
        hitbox.center = (pc.rb.transform.position + 10 * DirectionUtil.DirToVector(facing));
        hitbox.size = (new Vector3(10, 1, 10));

        // make a sphere to show hitbox
        myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer ren = myCube.GetComponents<Renderer>()[0];
        ren.material.color = Color.green;
        myCube.transform.localScale = new Vector3(10, 1, 10);
        myCube.transform.position = hitbox.center;
    }

    public override void FixedUpdate()
    {
        counter += 1;
        if (counter == donecount)
        {
            pc.stateEnded = true;
        }
    }

    public override void Update()
    {
        return;
    }

    // Destroy hitboxes
    public override void Exit()
    {
        GameObject.Destroy(hitbox);

        // Destry the sphere
        GameObject.Destroy(myCube);
    }
}
