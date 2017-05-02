using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public Vector3 dir;
	int donecount;
	int counter;
	float speed;
	int damage;

	void Awake ()
	{
		speed = 1.0f;
		donecount = 100;
		counter = 0;
	}

	// Use this for initialization
	void Start () {
		
	}


	void FixedUpdate()
	{
		gameObject.transform.position += speed * dir.normalized;

		counter += 1;
		if (counter >= donecount) 
		{
			GameObject.Destroy (gameObject);
		}
	}
}
