using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class EnemyController : MonoBehaviour {

	EnemyState currentState;
	public EnemyState nextState;
	public Vector3 movement { get; set; }
	public Rigidbody rb;
	public bool stateEnded { get; set; }
	public float maxSpeed;
	public float curSpeed { get; set; }
	public float targetRange = 30.0f;
	public float attackRange = 10.0f;
	public NavMeshAgent navAgent { get; set; }
	public GameObject ap;


	// render latch circle
	GameObject latchRadius;
	GameObject attackRadius;

	void Awake ()
	{
		maxSpeed = 20.0f;
		//variable initializations
		GameObject rootParent = this.transform.root.gameObject;
		if (rb == null)
		{
			rb = rootParent.GetComponent<Rigidbody>();
		}
		if (navAgent == null)
		{
			navAgent = rootParent.GetComponent<NavMeshAgent>();
		}
	}

	// Use this for initialization
	void Start () {
		rb.freezeRotation = true;
		currentState = new EnemyMovement.Targeting(this);

		latchRadius = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Renderer ren = latchRadius.GetComponents<Renderer> () [0];
		ren.material.color = Color.green;
		latchRadius.transform.localScale = new Vector3 (2*targetRange, .1f, 2*targetRange);

		attackRadius = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Renderer ren2 = attackRadius.GetComponents<Renderer> () [0];
		ren2.material.color = Color.red;
		attackRadius.transform.localScale = new Vector3 (2*attackRange, .1f, 2*attackRange);
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		latchRadius.transform.position = gameObject.transform.position + new Vector3(0, .2f, 0);

		attackRadius.transform.position = gameObject.transform.position + new Vector3(0, .4f, 0);

		//		Debug.Log(GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().currentHealth);
		//		Debug.Log(currentState);
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
