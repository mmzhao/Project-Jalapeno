using UnityEngine;

public class PlayerController : MonoBehaviour {

    PlayerState currentState;
    PlayerState nextState;
    public Vector3 movement;
    public Rigidbody rb;
    public bool stateEnded;
    public float maxSpeed;




    void Awake ()
    {

    }

	// Use this for initialization
	void Start () {
        rb = this.transform.root.gameObject.GetComponent<Rigidbody>();
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
        currentState.FixedUpdate();
        if (nextState != null)
        {
            stateEnded = false;
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            currentState.Enter();
        }
        print(currentState);
    }
}
