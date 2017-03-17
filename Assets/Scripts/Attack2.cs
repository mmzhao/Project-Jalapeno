using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : PlayerAttack {

    /** Fields from PlayerAttack:
     * PlayerController pc;
     * int counter;
     * int donecount;
     */
    BoxCollider hitbox;
    // make a sphere to show hitbox
    GameObject mySphere;
    public Attack2(PlayerController controller) : base(controller)
    {
        pc = controller;
        counter = 0;
        donecount = 8;
    }

    // Create hitboxes, start animation
    public override void Enter()
    {
        hitbox = pc.gameObject.AddComponent<BoxCollider>();
        Debug.Log(pc);
        Debug.Log(pc.rb);
        Debug.Log(pc.rb.transform.position);
        Debug.Log(hitbox);
        Debug.Log(hitbox.transform);
        Debug.Log(hitbox.transform.position);
        hitbox.transform.position = (pc.rb.transform.position + new Vector3(0, 0, 0));
        Debug.Log(pc.rb.transform.position);
        hitbox.size = (new Vector3(5, 5, 5));

        // make a sphere to show hitbox
		mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		GameObject.Destroy(mySphere.GetComponent<SphereCollider> ());
        Renderer ren = mySphere.GetComponents<Renderer>()[0];
		ren.material.color = Color.blue;
        mySphere.transform.localScale = new Vector3(30, 1, 30);
		mySphere.transform.position = hitbox.transform.position;
    }

    public override void FixedUpdate()
    {
        counter += 1;
        if (counter == donecount)
        {
            pc.stateEnded = true;
        }
    }

    public override void Update()
    {
        return;
    }

    // Destroy hitboxes
    public override void Exit()
    {
        GameObject.Destroy(hitbox);
        // Destry the sphere
        GameObject.Destroy(mySphere);
    }
}
