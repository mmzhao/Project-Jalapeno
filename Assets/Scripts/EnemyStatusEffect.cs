using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffect : MonoBehaviour {

    public class HitStun : EnemyState
    {
        public float stunTime;
        EnemyController ec;
        GameObject player;
        public Vector3 movement;
        public float curSpeed;
        public float targetRange;
        public float attackRange;

        public HitStun(EnemyController enemyController)
        {
            this.ec = enemyController;
            this.movement = enemyController.movement;
            this.curSpeed = enemyController.curSpeed;
            this.player = GameObject.FindGameObjectWithTag("Player");
            this.targetRange = enemyController.targetRange;
            this.attackRange = enemyController.attackRange;
            stunTime = ec.maxHitstun;
            ec.anim.SetInteger("state", 1);
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {
            ec.anim.SetInteger("state", 0);
        }

        public override void FixedUpdate()
        {
            stunTime -= Time.deltaTime;
            if (stunTime <= 0) ec.nextState = new EnemyMovement.Targeting(ec);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
