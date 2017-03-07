using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public Vector3 movement;
		public float curSpeed;
		public float targetRange;

		public Targeting(EnemyController enemyController)
		{
			this.ec = enemyController;
			this.movement = enemyController.movement;
			this.curSpeed = enemyController.curSpeed;
		}

		public override void Enter()
		{
			
		}

		public override void Exit()
		{

		}

		public override void FixedUpdate()
		{
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			if (player != null)
			{
//				Debug.Log (player.transform.position);
//				Debug.Log (ec.rb.position);
				Vector3 vecToPlayer = player.transform.position - ec.rb.position;
				if (vecToPlayer.magnitude <= ec.targetRange) 
				{
					Move (vecToPlayer);
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
