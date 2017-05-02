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
	public bool dead;
	public int hitstun;
	public int maxHitstun;

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
		targetRange = 70.0f;
		attackRange = 40.0f;
		hasLastPlayerPos = false;
		lastAttackTime = 0.0f;
		attackRechargeTime = 1.0f;
		dead = false;
		hitstun = 0;
		maxHitstun = 30;
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
		if (hitstun > 0 && !dead) {
			hitstun -= 1;
			if (hitstun < maxHitstun-1)
				rb.velocity = Vector3.zero;
		}
		else{
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
			if (nextState != null) {
				stateEnded = false;
				currentState.Exit ();
				currentState = nextState;
				nextState = null;
				currentState.Enter ();
			}
		}

		if (this.gameObject.GetComponent<Health>().currentHealth <= 0 && dead == false)
        {
//            currentState.Exit();
//            Destroy(this.gameObject);
//			this.gameObject.GetComponent<Health>().currentHealth = 0;
			dead = true;
			for (int i = 1; i < this.gameObject.transform.childCount; i++) {
				Destroy (this.gameObject.transform.GetChild(i).gameObject);
			}
			Destroy (this.gameObject.GetComponent<CapsuleCollider> ());
			currentState = new EnemyMovement.Death(this);
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

			rb.velocity = (gameObject.transform.position - other.transform.position).normalized * 200;
			if (currentState is EnemyAttack.Attack && this.gameObject.transform.childCount > 2) {
				Destroy (this.gameObject.transform.GetChild (this.gameObject.transform.childCount - 1).gameObject);
			}
			currentState = new EnemyMovement.Targeting (this);
			hitstun = maxHitstun;
        }
    }
}
