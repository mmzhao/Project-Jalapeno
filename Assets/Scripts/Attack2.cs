using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : PlayerAttack {
    
     /** Fields from PlayerAttack:
     * PlayerController pc;
     * Collider[] hitboxes;
     */

    public Attack2(PlayerController controller)
    {
        pc = controller;
    }

    // Create hitboxes, start animation
    public override void Enter()
    {
        return;
    }

    public override void FixedUpdate()
    {
        return;
    }

    public override void Update()
    {
        return;
    }

    // Destroy hitboxes
    public override void Exit()
    {
        return;
    }
}
