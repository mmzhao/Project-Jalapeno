using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyController : MonoBehaviour {

    public Rigidbody rb { get; set;}
    EnemyState currentState;
    EnemyState nextState;
    

	// Use this for initialization
	void Start () {
        if (rb == null)
        {
            rb = this.transform.root.gameObject.GetComponent<Rigidbody>();
        }
        rb.freezeRotation = true;

        currentState = new EnemyMovement.Idle(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
