using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour {

    PlayerState currentState;
    PlayerState nextState;
    public Vector3 movement { get; set; }
    public Rigidbody rb;
    public bool stateEnded { get; set; }
    public float maxSpeed = 40.0f;
    public float curSpeed { get; set; }

    void Awake ()
    {
        
    }

	// Use this for initialization
	void Start () {
        if (rb == null)
        {
            rb = this.transform.root.gameObject.GetComponent<Rigidbody>();
        }
        rb.freezeRotation = true;

        currentState = new PlayerMovement.Idle(this);
	}
	
	// Update is called once per frame
	void Update () {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        currentState.Update();
        if (stateEnded)
        {
            this.nextState = currentState.HandleInput();
        }
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
