using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour {
    public Vector3 dir;
	public float activeTime;
	private float counter = 0;
    private bool beginCounting;
    public float speed;

	void Awake ()
	{
		if (activeTime == 0) activeTime = 2.0f;
		counter = 0;
        beginCounting = false;
    }

	// Use this for initialization
	void Start ()
    {
    }

	void FixedUpdate()
	{
        if (beginCounting) counter += Time.deltaTime;
		if (counter >= activeTime) 
		{
			GameObject.Destroy (gameObject);
		}
	}

    public void launchProjectile(Vector3 dir, float speed)
    {
        GetComponent<Rigidbody>().velocity = speed * dir.normalized;
        beginCounting = true;
    }

    public void launchProjectile()
    {
        GetComponent<Rigidbody>().velocity = speed * dir.normalized;
        beginCounting = true;
    }
}
