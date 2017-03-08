﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	public class Targeting : EnemyState
	{
		EnemyController ec;
        GameObject player;
        NavMeshAgent navAgent;
		public Vector3 movement;
		public float curSpeed;
		public float targetRange;
		public float attackRange;

		public Targeting(EnemyController enemyController)
		{
			this.ec = enemyController;
			this.movement = enemyController.movement;
			this.curSpeed = enemyController.curSpeed;
            this.player = GameObject.FindGameObjectWithTag("Player");
            this.navAgent = enemyController.navAgent;
			this.targetRange = enemyController.targetRange;
			this.attackRange = enemyController.attackRange;
        }

		public override void Enter()
		{
			
		}

		public override void Exit()
		{
			navAgent.SetDestination(ec.transform.position);
		}

		public override void FixedUpdate()
		{
			if (player != null)
			{
//				Debug.Log (player.transform.position);
//				Debug.Log (ec.rb.position);
				Vector3 vecToPlayer = player.transform.position - ec.rb.position;
				if (vecToPlayer.magnitude <= targetRange) 
				{
                    //					Move (vecToPlayer);
                    navAgent.SetDestination(player.transform.position);
				} else
                {
                    navAgent.SetDestination(ec.transform.position);
                }
			}

			if (player != null)
			{
				Vector3 vecToPlayer = player.transform.position - ec.rb.position;
				if (vecToPlayer.magnitude <= attackRange) 
				{
					ec.nextState = new EnemyAttack.Attack(ec, vecToPlayer.normalized);
				}
			}
		}

		public override void Update()
		{

        }

		public void Move (Vector3 dir)
		{
			Vector3 dif = dir.normalized * ec.maxSpeed * Time.deltaTime;
			ec.rb.MovePosition (ec.transform.position + dif);
		}


	}
}
