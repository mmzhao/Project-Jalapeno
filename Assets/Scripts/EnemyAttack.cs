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
		int donecount = 40;
		int counter;
		GameObject attack;

		public Attack(EnemyController enemyController, Vector3 dir)
		{
			this.ec = enemyController;
			attackDir = dir;
			attackRange = enemyController.attackRange;
			counter = 0;
		}

		public override void Enter()
		{
			attack = (GameObject) Instantiate(ec.ap);
			attack.transform.position = ec.transform.position;
			attack.transform.localEulerAngles = new Vector3 (0, -Mathf.Atan2 (attackDir.z, attackDir.x) * 180f / Mathf.PI, 0);
			foreach (Transform hitbox in attack.transform) 
			{
				hitbox.gameObject.SetActive (false);
                hitbox.GetComponent<Collider>().isTrigger = false;
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
				if (counter / (donecount / 5) == hitboxIndex) {
//					Debug.Log (hitboxIndex + " True");
					hitbox.gameObject.SetActive (true);
				} else {
//					Debug.Log (hitboxIndex + " False");
					hitbox.gameObject.SetActive (false);
				}
				hitboxIndex++;
			}

			counter++;
			//			Debug.Log ("attack " + curMoves);
//			GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);

			if (counter >= donecount) 
			{
				ec.nextState = new EnemyMovement.Targeting (ec);
			}
		}

		public override void Update()
		{

		}
	}
}
