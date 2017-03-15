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
		int numMoves = 20;
		int curMoves;
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
			attack = (GameObject) Instantiate(ec.ap);
			attack.transform.position = ec.transform.position;
			attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (attackDir.z, attackDir.x) * 180f / Mathf.PI, 0);
			foreach (Transform hitbox in attack.transform) 
			{
				hitbox.gameObject.SetActive (false);
			}
		}

		public override void Exit()
		{
			Destroy(attack);
		}

		public override void FixedUpdate()
		{
			int hitboxIndex = 0;
			foreach (Transform hitbox in attack.transform) 
			{
//				Debug.Log (curMoves + " " + numMoves + " " + curMoves / (numMoves / 5));
				if (curMoves / (numMoves / 5) == hitboxIndex) {
//					Debug.Log (hitboxIndex + " True");
					hitbox.gameObject.SetActive (true);
				} else {
//					Debug.Log (hitboxIndex + " False");
					hitbox.gameObject.SetActive (false);
				}
				hitboxIndex++;
			}

			curMoves++;
			//			Debug.Log ("attack " + curMoves);
//			GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);

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
