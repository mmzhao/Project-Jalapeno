using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {

	public class Attack : EnemyState
	{
		BoxCollider hitbox;
		GameObject[] hitboxes;
		float[] activateHitboxMoments = { 0f }; // these mark the timestamp to move to the next hitbox
		float[] deactivateHitboxMoments = { .5f};
		Vector3 facing;
		int numHitboxes;
		int activateHitboxIndex = 0;
		int deactivateHitboxIndex = 0;
		int damage;
		protected float attackDuration;
		protected float attackTimer;

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
			attackTimer = 0;
			numHitboxes = activateHitboxMoments.Length;
			hitboxes = new GameObject[numHitboxes];
			damage = 30;
		}

		public override void Enter()
		{
			attack = (GameObject) GameObject.Instantiate(ec.ap);
			damage = attack.GetComponent<AttackVariables>().Damage();
			attack.transform.parent = ec.transform;
			attack.transform.position = ec.transform.position + new Vector3(0, 2, 0) + facing.normalized*8;
			attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (facing.z, facing.x) * 180f / Mathf.PI, 0);
			Transform hitboxContainer = findHitboxesByTag(attack.transform);
			for (int i = 0; i < numHitboxes; i++)
			{
				hitboxes[i] = hitboxContainer.GetChild(i).gameObject;
				Debug.Log (hitboxes [i]);
//				hitboxes[i].SetActive(false);
			}
		}

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

		// Destroy hitboxes
		public override void Exit()
		{
//			GameObject.Destroy(attack);
		}

		public override void FixedUpdate()
		{
			//			base.FixedUpdate();
			attackTimer += Time.deltaTime;
//			while (activateHitboxIndex < numHitboxes && attackTimer >= activateHitboxMoments[activateHitboxIndex])
//			{
//				hitboxes[activateHitboxIndex].SetActive(true);
//				activateHitboxIndex++;
//			}
//
//			while (deactivateHitboxIndex < numHitboxes && attackTimer >= deactivateHitboxMoments[deactivateHitboxIndex])
//			{
//				hitboxes[deactivateHitboxIndex].SetActive(false);
//				deactivateHitboxIndex++;
//			}     
			if (attackTimer > attackDuration) 
			{
				attack.GetComponent<ProjectileController>().dir = facing;
				ec.nextState = new EnemyMovement.Targeting (ec);
			}
		}

		public override void Update()
		{

		}
	}
}
