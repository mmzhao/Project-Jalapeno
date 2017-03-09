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
	public float targetRange = 30.0f;
	public float attackRange = 10.0f;
    public NavMeshAgent navAgent { get; set; }
    

	// render latch circle
	LineRenderer lineRenderer;

	void Awake ()
	{
		maxSpeed = 20.0f;


		// latch/attack range circle setup
		float theta_scale = 0.1f;             //Set lower to add more points
		int size = (int) ((2.0f * Mathf.PI) / theta_scale) + 2; //Total number of points in circle.
		size *= 2;
		
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		//		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.2f, 0.2f);
		lineRenderer.SetVertexCount(size);
        // latch/attack range circle setup

        //variable initializations
        GameObject rootParent = this.transform.root.gameObject;
        if (rb == null)
        {
            rb = rootParent.GetComponent<Rigidbody>();
        }
        if (navAgent == null)
        {
            navAgent = rootParent.GetComponent<NavMeshAgent>();
        }
    }

	// Use this for initialization
	void Start () {
        rb.freezeRotation = true;
        currentState = new EnemyMovement.Targeting(this);
	}
    
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		// draw latch/attack range circle
		float theta_scale = 0.1f;
		float x,x2 = 0;
		float z,z2 = 0;
		float r = targetRange;
		float r2 = attackRange;

		int i = 0;
		for(float theta = 0; theta-theta_scale < 2 * Mathf.PI; theta += theta_scale) {
			x = r*Mathf.Cos(theta) + rb.position.x;
			z = r*Mathf.Sin(theta) + rb.position.z;
			x2 = r2*Mathf.Cos(theta) + rb.position.x;
			z2 = r2*Mathf.Sin(theta) + rb.position.z;
			Vector3 pos = new Vector3(x, .5f, z);
			Vector3 pos2 = new Vector3(x2, .5f, z2);
			lineRenderer.SetPosition(i, pos);
			i+=1;
			lineRenderer.SetPosition(i, pos2);
			i+=1;
		}
        // draw latch/attack range circle


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
	}
}
