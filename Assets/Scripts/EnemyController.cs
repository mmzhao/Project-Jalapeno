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
	public GameObject projectile;
    public Animator anim;

	// render latch circle
	GameObject latchRadius;
	GameObject attackRadius;
	public Vector3 lastPlayerPos;
	public bool hasLastPlayerPos;

	public float lastAttackTime;
	public float attackRechargeTime;

	void Awake ()
	{
		maxSpeed = 50.0f;
		targetRange = 50.0f;
		attackRange = 20.0f;
		hasLastPlayerPos = false;
		lastAttackTime = 0.0f;
		attackRechargeTime = 1.0f;
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

		//latchRadius = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//Renderer ren = latchRadius.GetComponents<Renderer> () [0];
		//ren.material.color = Color.green;
		//latchRadius.transform.localScale = new Vector3 (2*targetRange, .1f, 2*targetRange);
		//Destroy(latchRadius.GetComponent<SphereCollider> ());

		//attackRadius = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//Renderer ren2 = attackRadius.GetComponents<Renderer> () [0];
		//ren2.material.color = Color.red;
		//attackRadius.transform.localScale = new Vector3 (2*attackRange, .1f, 2*attackRange);
		//Destroy(attackRadius.GetComponent<SphereCollider> ());
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
//		Debug.Log (currentState);
		rb.velocity = Vector3.zero;
		if (lastAttackTime > 0.0f) 
		{
			lastAttackTime -= Time.deltaTime;
		}
		//latchRadius.transform.position = gameObject.transform.position + new Vector3(0, .2f, 0);

		//attackRadius.transform.position = gameObject.transform.position + new Vector3(0, .4f, 0);

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


    public void getHit(GameObject go, Collider other)
    {
        if (other.gameObject.transform.root.tag == "Player")
        {
            Health h = GetComponent<Health>();
            Transform t = other.transform;
            while (t.parent != t.root) t = t.parent;

            int dmg = t.GetComponent<AttackVariables>().Damage();
            h.TakeDamage(dmg, t);
        }
    }
}
