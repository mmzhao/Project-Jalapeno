using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour {

    PlayerState currentState;
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
	public int attack1Charges;
	public int attack2Charges;
	public int dashCharges;

	public Animator anim;
	public Camera playerCamera;


    public Vector3 playerToMouse { get; set; }
    void Awake ()
    {
		maxSpeed = 50.0f;
		dashSpeed = 200.0f;

		attack1Charges = 100;
		attack2Charges = 100;
		dashCharges = 100;

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
    }

	void OnTriggerEnter (Collider other)
	{
//		Debug.Log ("e");
//		Debug.Log (other.gameObject.transform.parent.name.Substring(0, 11));
//		Debug.Log(other.gameObject.transform.parent.name.Length);
		if(other.gameObject.transform.parent.name.Length >= 10 &&
			other.gameObject.transform.parent.name.Substring(0, 11) == "EnemyAttack")
		{
//			Debug.Log ("hit " + other.gameObject.name);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);
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

}
