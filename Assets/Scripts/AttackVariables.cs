using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVariables : MonoBehaviour {

    public int damage;
    bool hit;

	// Use this for initialization
	void Start () {
        hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int Damage()
    {
        return damage;
    }

    public bool Hit()
    {
        return hit;
    }

    public void ToggleHit()
    {
        hit = true;
    }

}
