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

    public Attack1(PlayerController controller)
    {
        pc = controller;
        counter = 0;
        donecount = 10;
    }

    // Create hitboxes, start animation
    public override void Enter()
    {
        hitbox = new BoxCollider();
        hitbox.center = (new Vector3(0, 0, 0));
        hitbox.size = (new Vector3(5, 5, 5));
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
    }
}
