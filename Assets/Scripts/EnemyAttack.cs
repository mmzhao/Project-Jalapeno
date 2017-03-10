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
		GameObject hitBox;
		GameObject attack;

		public Attack(EnemyController enemyController, Vector3 dir)
		{
			this.ec = enemyController;
			attackDir = dir;
			attackRange = enemyController.attackRange;
			curMoves = 0;
		}

		public override void Enter()
		{
//			hitBox = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//			Renderer ren = hitBox.GetComponents<Renderer> () [0];
//			ren.material.color = Color.red;
//			hitBox.transform.localScale = new Vector3 (10, 1, 10);
//			hitBox.transform.position = ec.transform.position + attackDir.normalized * attackRange;
			attack = (GameObject) Instantiate(ec.ap);
			attack.transform.position = ec.transform.position;
//			Debug.Log (Mathf.Atan2 (attackDir.z, attackDir.x));
//			Debug.Log (new Vector2(attackDir.x, attackDir.z));
//			Debug.Log (Vector2.Angle (new Vector2 (0, 0), new Vector2(attackDir.x, attackDir.z)));
			attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (attackDir.z, attackDir.x) * 180f / Mathf.PI, 0);
		}

		public override void Exit()
		{
//			Destroy (hitBox);
			Destroy(attack);
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
