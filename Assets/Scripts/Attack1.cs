using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : PlayerAttack {
    
    /** Fields from PlayerAttack:
     * PlayerController pc;
     * Collider[] hitboxes;
     */

    public Attack1(PlayerController controller)
    {
        pc = controller;
    }

    // Create hitboxes, start animation
    public override void Enter();

    public override void FixedUpdate();

    public override void Update();

    // Destroy hitboxes
    public override void Exit();
}
