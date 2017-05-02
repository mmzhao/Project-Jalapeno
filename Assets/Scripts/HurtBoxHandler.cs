using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Created by Eric Zhou
 * Handles triggers for hurtboxes and passes them to PlayerController
 * Handles Hurtbox initialization for further idiotproofing
 */
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HurtBoxHandler : MonoBehaviour {
    private PlayerController pc;
    private EnemyController ec;
    private BossController bc;
    public bool forEnemy;
    public bool forBoss;

    void Awake ()
    {
        if (forEnemy)
        {
			ec = transform.parent.parent.GetComponent<EnemyController>();
//			Debug.Log (transform.parent.parent);
        }
        if (forBoss)
            bc = transform.parent.parent.GetComponent<BossController>();
        else
        {
            pc = transform.root.GetComponent<PlayerController>();
        }
        Collider col = gameObject.GetComponent<Collider>();
        col.isTrigger = true;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        if (gameObject.tag == "Shield")
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
	{
        if (!(forEnemy || forBoss))
        {
            pc.getHit(gameObject, other);
        }
        else
        {
//			Debug.Log (ec);
            if (forBoss)
            {
                bc.getHit(gameObject, other);
                return;
            }
            ec.getHit(gameObject, other);
        }
    }
}
