using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {

	public class Attack : EnemyState
	{
		Vector3 facing;
		int damage;
		protected float attackDuration; // charge time before enemy launches projectile
        protected float attackCooldown; // time to cool down after firing
		protected float attackTimer;
        public float projectileSpeed = 40;
        public bool attackLaunched;

		EnemyController ec;

		protected static string HITBOX_CONTAINER_TAG = "HitboxContainer";

		// make a cube to show hitbox
		//	GameObject myCube;
		GameObject attack;

		public Attack(EnemyController enemyController, Vector3 dir)
		{
			this.ec = enemyController;
			facing = dir.normalized;
			facing.y = 0;
			attackDuration = 0.5f;
            attackCooldown = 0.5f;
			attackTimer = 0;
			damage = 30;
		}

		public override void Enter()
		{
			attack = (GameObject) GameObject.Instantiate(ec.ap);
            attack.transform.parent = ec.transform;
			damage = attack.GetComponent<AttackVariables>().Damage();
			attack.transform.position = ec.transform.position + new Vector3(0, 2, 0) + facing.normalized*8;
			attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (facing.z, facing.x) * 180f / Mathf.PI, 0);
		}

		// Destroy hitboxes
		public override void Exit()
		{
            if (!attackLaunched) Destroy(attack);
		}

		public override void FixedUpdate()
		{
			attackTimer += Time.deltaTime;
            if (attackLaunched)
            {
                if (attackTimer > attackCooldown + attackDuration)
                {
                    ec.nextState = new EnemyMovement.Targeting(ec);
                }
            }
            else
            {
                if (attackTimer > attackDuration)
                {
                    attack.transform.parent = null;
                    attack.GetComponent<ProjectileController>().launchProjectile(facing, projectileSpeed);
                    attackLaunched = true;
                }
            }
        }

		public override void Update()
		{

		}
	}
}
