using UnityEngine;
using UnityEngine.UI;


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

    public GameObject hurtBoxes;

	//	Attack prefabs
	public GameObject ap1;
    public GameObject ap2;

    //attack parameters
    public float attack1Charges { get; set; }
    public float attack2Charges { get; set; }
    public float dashCharges { get; set; }
    public float shieldTime { get; set; }
	public bool shieldBroken { get; set; }

    public float maxAttack1Charges;
    public float maxAttack2Charges;
    public float maxDashCharges;
    public float maxShieldTime;

    public int attack1ChargeRate; // amount of charges to recover in 1 second
    public int attack2ChargeRate;
    public int dashChargeRate;
    public float shieldChargeRate; // multiplier for shield charge rate. Normal rate is 1 unit per second.

	public Text text;


    public Animator anim;
	public Camera playerCamera;

    public Vector3 playerToMouse { get; set; }
    void Awake ()
    {
		if (maxSpeed == 0) maxSpeed = 50.0f;
        if (dashSpeed == 0) dashSpeed = 100.0f;
        if (maxAttack1Charges == 0) maxAttack1Charges = 5;
        if (maxAttack2Charges == 0) maxAttack2Charges = 5;
        if (maxDashCharges == 0) maxDashCharges = 5;
        if (maxShieldTime == 0) maxShieldTime = 5; // in seconds  

        attack1Charges = maxAttack1Charges;
		attack2Charges = maxAttack2Charges;
		dashCharges = maxDashCharges;
        shieldTime = maxShieldTime;
		shieldBroken = false;

        if (attack1ChargeRate == 0) attack1ChargeRate = 1;
        if (attack2ChargeRate == 0) attack2ChargeRate = 1;
        if (dashChargeRate == 0) dashChargeRate = 1;
        if (shieldChargeRate == 0) shieldChargeRate = 1;

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

        if (hurtBoxes == null)
        {
            Transform hurtTransform = transform.Find("HurtBoxes");
            foreach (Collider coll in hurtTransform.GetComponentsInChildren<Collider>())
            {
                coll.isTrigger = true;
            }
            hurtBoxes = hurtTransform.gameObject;
        }
        
    }

	// Use this for initialization
	void Start () {
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
//		rb.constraints = RigidbodyConstraints.FreezeRotation;

        currentState = new PlayerMovement.Idle(this);
	}
	
	// Update is called once per frame
	void Update () {
		//text.text = "Attack1 Charges: " + (int) attack1Charges + " " + "Attack2 Charges: " + (int) attack2Charges + " " + "Dash Charges: " + (int) dashCharges;
//		Debug.Log (text.text);

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

        if (this.GetComponent<Health>().currentHealth <= 0)
        {
            this.currentState.Exit();
            Destroy(this.gameObject);
        }

    }

    void FixedUpdate()
    {
//		Debug.Log (currentState);
        //		Debug.Log(GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().currentHealth);
        //		GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);
		//rb.velocity = Vector3.zero;
	
        if (nextState != null)
        {
            stateEnded = false;
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            currentState.Enter();
        }
        
        currentState.FixedUpdate();
        rechargeMoves();
    }

    public void getHit (GameObject go, Collider other)
    {
        if (go.tag == "Shield")
        {
            shieldTime -= 1;
        }
        else
        {
//			Debug.Log (other.gameObject.transform.root.name);
//			Debug.Log (other.gameObject.transform.parent.name);
//			Debug.Log (other.gameObject.transform.parent.parent.parent.name);
			if (other.gameObject.transform.parent.parent.parent.name.Length >= 5 &&
				other.gameObject.transform.parent.parent.parent.name.Substring(0, 5) == "Enemy")
            {
				if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().damaged && !other.gameObject.transform.parent.parent.GetComponent<AttackVariables>().Hit())
                {
					other.gameObject.transform.parent.parent.GetComponent<AttackVariables>().ToggleHit();
					GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().TakeDamage(other.gameObject.transform.parent.parent.GetComponent<AttackVariables>().Damage());
                }
            }
        }
    }

	public bool canAttack1()
	{
		return attack1Charges >= 1;
	}

	public bool canAttack2()
	{
		return attack2Charges >= 1;
	}

	public bool canDash()
	{
		return dashCharges >= 1;
	}

    public bool canShield()
    {
		return shieldTime >= 1 && !shieldBroken;
    }

    private void rechargeMoves ()
    {
        if (currentState.getPlayerStateIndex() != PlayerStateIndex.SHIELD)
        {
            if (shieldTime >= maxShieldTime)
            {
                shieldTime = maxShieldTime;
				shieldBroken = false;
            }
            else
            {
                shieldTime += Time.deltaTime * shieldChargeRate;
            }
        }
        if (attack1Charges < maxAttack1Charges)
        {
            attack1Charges += Time.deltaTime * attack1ChargeRate;
			if (attack1Charges > maxAttack1Charges)
				attack1Charges = maxAttack1Charges;
        }
        if (attack2Charges < maxAttack2Charges)
        {
            attack2Charges += Time.deltaTime * attack2ChargeRate;
			if (attack2Charges > maxAttack2Charges)
				attack2Charges = maxAttack2Charges;
        }
        if (dashCharges < maxDashCharges)
        {
            dashCharges += Time.deltaTime * dashChargeRate;
			if (dashCharges > maxDashCharges)
				dashCharges = maxDashCharges;
        }
    }

}
