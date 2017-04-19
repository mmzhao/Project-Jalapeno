using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : PlayerState {

    protected float attackTimer;
    protected float attackDuration;
    protected int counter;
    protected int donecount;
    protected float cancellableHitboxTime;

    protected static string HITBOX_CONTAINER_TAG = "HitboxContainer";

    
    public PlayerAttack(PlayerController pc) : base(pc)
    {
        attackTimer = 0;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDuration)
        {
            pc.stateEnded = true;
        }
    }

    // Destroy hitboxes
    public override abstract void Exit();

    protected Transform findHitboxesByTag(Transform attackPrefab)
    {
        foreach (Transform child in attackPrefab)
        {
            if (child.tag == HITBOX_CONTAINER_TAG)
            {
                return child;
            }
        }
        return null;
    }
}
