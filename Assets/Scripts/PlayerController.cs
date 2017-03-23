using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour {

    public PlayerState currentState;
    public PlayerState nextState;

    public Vector3 movementInput { get; set; }
    public Rigidbody rb;
    public bool stateEnded { get; set; }
    public float maxSpeed;
    public float curSpeed { get; set; }
    public float dashSpeed;
    public Direction facingDirection = Direction.NW;
    public Direction movingDirection = Direction.NW;
	//	Attack prefabs
	public GameObject ap1;
	public GameObject ap2;

	public float attack1Charges { get; set; }
    public float attack2Charges { get; set; }
    public float dashCharges { get; set; }
    public float shieldTime { get; set; }

    public float maxAttack1Charges;
    public float maxAttack2Charges;
    public float maxDashCharges;
    public float maxShieldTime;

    public int attack1ChargeRate; // amount of charges to recover in 1 second
    public int attack2ChargeRate;
    public int dashChargeRate;
    public float shieldChargeRate; // multiplier for shield charge rate. Normal rate is 1 unit per second.




    public Animator anim;
	public Camera playerCamera;
//    public Collider playerShield;

    public Vector3 playerToMouse { get; set; }
    void Awake ()
    {
		maxSpeed = 50.0f;
		dashSpeed = 100.0f;

        maxAttack1Charges = 5;
        maxAttack2Charges = 2;
        maxDashCharges = 5;
        maxShieldTime = 5; // in seconds

        attack1Charges = maxAttack1Charges;
		attack2Charges = maxAttack2Charges;
		dashCharges = maxDashCharges;
        shieldTime = maxShieldTime;

        attack1ChargeRate = 1;
        attack2ChargeRate = 1;
        dashChargeRate = 1;
        shieldChargeRate = 1;

        if (rb == null)
        {
            rb = this.transform.root.gameObject.GetComponent<Rigidbody>();
        }
        if (anim == null)
        {
            anim = this.transform.root.gameObject.GetComponent<Animator>();
        }
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

//        if (playerShield == null)
//        {
//            playerShield = transform.FindChild("Shield").GetComponent<Collider>();
//        }

    }

	// Use this for initialization
	void Start () {
//		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		rb.constraints = RigidbodyConstraints.FreezeRotation;

        currentState = new PlayerMovement.Idle(this);
	}
	
	// Update is called once per frame
	void Update () {
        //register all the inputs that need to be dynamically tracked
        //movement key inputs
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //mouse position
        Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, playerCamera.farClipPlane, LayerMask.GetMask("MouseRaycast")))
        {
            Vector3 p2m = floorHit.point - transform.position;
            p2m.y = 0f;
            playerToMouse = p2m.normalized;
        }

        //Animate
        currentState.Animate();

        //carry out state-specific orders
        currentState.Update();
        if (stateEnded)
        {
			if (nextState == null)
	            this.nextState = currentState.HandleInput();
			stateEnded = false;
        }
    }

    void FixedUpdate()
    {
//		Debug.Log (currentState);
        //		Debug.Log(GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().currentHealth);
        //		GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);
		rb.velocity = Vector3.zero;
		currentState.FixedUpdate();
        if (nextState != null)
        {
            stateEnded = false;
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            currentState.Enter();
        }

        rechargeMoves();
    }

	void OnTriggerEnter (Collider other)
	{
//		Debug.Log ("e");
//		Debug.Log (other.gameObject.transform.parent.name.Substring(0, 11));
//		Debug.Log(other.gameObject.transform.parent.name.Length);
        if (currentState.playerState != PlayerStateIndex.SHIELD)
        {
            if (other.gameObject.transform.parent.name.Length >= 10 &&
                other.gameObject.transform.parent.name.Substring(0, 11) == "EnemyAttack")
            {
                //			Debug.Log ("hit " + other.gameObject.name);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().TakeDamage(1);
            }

        }
    }

	public bool canAttack1()
	{
		return attack1Charges > 0;
	}

	public bool canAttack2()
	{
		return attack2Charges > 0;
	}

	public bool canDash()
	{
		return dashCharges > 0;
	}

    private void rechargeMoves ()
    {
        if (currentState.playerState != PlayerStateIndex.SHIELD)
        {
            if (shieldTime > maxShieldTime)
            {
                shieldTime = maxShieldTime;
            }
            else
            {
                shieldTime += Time.deltaTime * shieldChargeRate;
            }
        }
        if (attack1Charges < maxAttack1Charges)
        {
            attack1Charges += Time.deltaTime * attack1ChargeRate;
        }
        if (attack2Charges < maxAttack2Charges)
        {
            attack2Charges += Time.deltaTime * attack2ChargeRate;
        }
        if (dashCharges < maxDashCharges)
        {
            dashCharges += Time.deltaTime * dashChargeRate;
        }
    }

}
