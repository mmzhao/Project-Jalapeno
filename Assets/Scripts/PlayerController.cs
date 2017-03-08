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
    public enum Direction { N, NE, E, SE, S, SW, W, NW };
    public Direction facing = Direction.NW;

    void Awake ()
    {
		maxSpeed = 40.0f;
		dashSpeed = 200.0f;
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
		// Debug.Log(currentState);
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

    // Calculates (vertical + 3 * horizontal); outputs the corresponding direction. Assumes not 0 (returns NW by default).
    public Direction FloatToDir(float vertical, float horizontal)
    {
        float input = vertical + 3 * horizontal;
        if (input == 1)
        {
            return Direction.N;
        } else if (input == 4)
        {
            return Direction.NE;
        }
        else if (input == 3)
        {
            return Direction.E;
        }
        else if (input == 2)
        {
            return Direction.SE;
        }
        else if (input == -1)
        {
            return Direction.S;
        }
        else if (input == -4)
        {
            return Direction.SW;
        }
        else if (input == -3)
        {
            return Direction.W;
        }
        else
        {
            return Direction.NW;
        }
    }
}
