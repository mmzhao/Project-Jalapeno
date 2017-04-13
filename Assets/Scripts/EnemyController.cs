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
	public float targetRange;
	public float attackRange;
	public NavMeshAgent navAgent { get; set; }
	public GameObject ap;
    public Animator anim;

	// render latch circle
	GameObject latchRadius;
	GameObject attackRadius;
	public Vector3 lastPlayerPos;
	public bool hasLastPlayerPos;

	void Awake ()
	{
		maxSpeed = 20.0f;
		targetRange = 50.0f;
		attackRange = 20.0f;
		hasLastPlayerPos = false;
		//variable initializations
		GameObject rootParent = this.transform.gameObject;
		if (rb == null)
		{
			rb = rootParent.GetComponent<Rigidbody>();
		}
		if (navAgent == null)
		{
			navAgent = rootParent.GetComponent<NavMeshAgent>();
		}
        if (anim == null)
        {
            anim = rootParent.GetComponent<Animator>();
        }
    }

	// Use this for initialization
	void Start () {
		//		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		currentState = new EnemyMovement.Targeting(this);

		latchRadius = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Renderer ren = latchRadius.GetComponents<Renderer> () [0];
		ren.material.color = Color.green;
		latchRadius.transform.localScale = new Vector3 (2*targetRange, .1f, 2*targetRange);
		Destroy(latchRadius.GetComponent<SphereCollider> ());

		attackRadius = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Renderer ren2 = attackRadius.GetComponents<Renderer> () [0];
		ren2.material.color = Color.red;
		attackRadius.transform.localScale = new Vector3 (2*attackRange, .1f, 2*attackRange);
		Destroy(attackRadius.GetComponent<SphereCollider> ());
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

        if (this.gameObject.GetComponent<Health>().currentHealth <= 0)
        {
            currentState.Exit();
            Destroy(this.gameObject);
        }
	}

	void OnTriggerEnter (Collider other)
	{
//		Debug.Log ("e");
		//		Debug.Log (other.gameObject.transform.parent.name.Substring(0, 11));
		if (other.gameObject.transform.parent.name.Length >= 9 &&
		    other.gameObject.transform.parent.name.Substring(0, 8) == "MCAttack")
		{
//			Debug.Log ("hit " + other.gameObject.name);
		  this.transform.gameObject.GetComponent<Health> ().TakeDamage (other.gameObject.transform.parent.GetComponent<AttackVariables>().Damage());
		}
	}
}
