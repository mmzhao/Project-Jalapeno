using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour {

    PlayerState currentState;
    PlayerState nextState;

    public Vector3 movement { get; set; }
    public Rigidbody rb;
    public bool stateEnded { get; set; }
    public float maxSpeed;
    public float curSpeed { get; set; }
    public float dashSpeed;
    public Direction facing = Direction.NW;
    public Animator anim;

    void Awake ()
    {
		maxSpeed = 50.0f;
		dashSpeed = 200.0f;

        if (rb == null)
        {
            rb = this.transform.root.gameObject.GetComponent<Rigidbody>();
        }
        if (anim == null)
        {
            anim = this.transform.root.gameObject.GetComponent<Animator>();
        }
    }

	// Use this for initialization
	void Start () {
        
        rb.freezeRotation = true;

        currentState = new PlayerMovement.Idle(this);
	}
	
	// Update is called once per frame
	void Update () {
        // Debug.Log(currentState);
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector3(moveX, 0, moveY);

        if (moveX != 0 || moveY != 0)
        {
            anim.SetFloat("LastInputX", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("LastInputY", Input.GetAxisRaw("Vertical"));
        }
        currentState.Update();
        if (stateEnded)
        {
            this.nextState = currentState.HandleInput();
        }
    }

    void FixedUpdate()
    {


        //		Debug.Log(GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().currentHealth);
        //		GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);
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
		if(other.gameObject.transform.parent.name.Substring(0, 11) == "EnemyAttack")
		{
//			Debug.Log ("hit " + other.gameObject.name);
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().TakeDamage (1);
		}
	}

}
