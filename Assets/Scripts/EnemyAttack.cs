using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {
	
	public class Attack : EnemyState
	{
		EnemyController ec;
		Vector3 attackDir;
		public float attackRange;
		int numMoves = 10;
		int curMoves;
		GameObject mySphere;

		public Attack(EnemyController enemyController, Vector3 dir)
		{
			this.ec = enemyController;
			attackDir = dir;
			attackRange = enemyController.attackRange;
			curMoves = 0;
		}

		public override void Enter()
		{
			mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Renderer ren = mySphere.GetComponents<Renderer> () [0];
			ren.material.color = Color.green;
			mySphere.transform.localScale = new Vector3 (10, 1, 10);
			mySphere.transform.position = ec.transform.position + attackDir.normalized * attackRange;
		}

		public override void Exit()
		{
			Destroy (mySphere);
		}

		public override void FixedUpdate()
		{
			curMoves++;
//			Debug.Log ("attack " + curMoves);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);

			if (curMoves >= numMoves) 
			{
				ec.nextState = new EnemyMovement.Targeting (ec);
			}
		}

		public override void Update()
		{

		}

	}
}
