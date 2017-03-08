using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

//	public class Idle : EnemyState
//	{
//		EnemyController ec;
//
//		public Idle(EnemyController enemyController)
//		{
//			this.ec = enemyController;
//		}
//
//		public override void Enter()
//		{
//			ec.curSpeed = 0;
//		}
//
//		public override void Exit()
//		{
//			
//		}
//
//		public override void FixedUpdate()
//		{
//			if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
//			{
//				ec.stateEnded = true;
//			}
//		}
//
//		public override PlayerState HandleInput()
//		{
//			if (ec.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
//			{
//				return new Running(pc);
//			}
//			return null;
//		}
//
//		public override void Update()
//		{
//
//		}
//	}

	public class Targeting : EnemyState
	{
		EnemyController ec;
        GameObject player;
        NavMeshAgent navAgent;
		public Vector3 movement;
		public float curSpeed;
		public float targetRange;

		public Targeting(EnemyController enemyController)
		{
			this.ec = enemyController;
			this.movement = enemyController.movement;
			this.curSpeed = enemyController.curSpeed;
            this.player = GameObject.FindGameObjectWithTag("Player");
            this.navAgent = enemyController.navAgent;
        }

		public override void Enter()
		{
			
		}

		public override void Exit()
		{

		}

		public override void FixedUpdate()
		{
			if (player != null)
			{
				Vector3 vecToPlayer = player.transform.position - ec.transform.position;
                print(vecToPlayer.magnitude);
                if (vecToPlayer.magnitude <= ec.targetRange) 
				{

                    //					Move (vecToPlayer);
                    navAgent.SetDestination(player.transform.position);
				} else
                {
                    navAgent.SetDestination(ec.transform.position);
                }
			}

		}

		public override void Update()
		{
            navAgent.SetDestination(player.transform.position);
        }

		public void Move (Vector3 dir)
		{
			Vector3 dif = dir.normalized * ec.maxSpeed * Time.deltaTime;
			ec.rb.MovePosition (ec.transform.position + dif);
		}


	}
}
