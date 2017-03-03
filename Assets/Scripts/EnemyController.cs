using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class EnemyController : MonoBehaviour {

	EnemyState currentState;
	EnemyState nextState;
	public Vector3 movement { get; set; }
	public Rigidbody rb;
	public bool stateEnded { get; set; }
	public float maxSpeed;
	public float curSpeed { get; set; }

	void Awake ()
	{
		maxSpeed = 20.0f;
	}

	// Use this for initialization
	void Start () {
		if (rb == null)
		{
			rb = this.transform.root.gameObject.GetComponent<Rigidbody>();
		}
		rb.freezeRotation = true;

		currentState = new EnemyMovement.Targeting(this);
	}

	// Update is called once per frame
	void Update () {
		movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		currentState.Update();
	}

	void FixedUpdate()
	{
		//		Debug.Log(GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().currentHealth);
		currentState.FixedUpdate();
		if (nextState != null)
		{
			stateEnded = false;
			currentState.Exit();
			currentState = nextState;
			nextState = null;
			currentState.Enter();
		}
	}
}
