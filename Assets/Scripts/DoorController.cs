using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    string doortag;
    GameObject[] room;
    GameObject enemies;

    void Awake()
    {
        doortag = this.gameObject.tag;
        room = GameObject.FindGameObjectsWithTag(doortag);
        int i = 0;
        for (; i < 2; i += 1)
        {
            if (room[i].GetComponentInChildren<Rigidbody>() != null)
            {
                enemies = room[i];
            }
        }
        Debug.Log(enemies);
    }

    // Update is called once per frame
    void Update () {
        if (enemies.GetComponentInChildren<Rigidbody>() == null)
        {
            Destroy(this.gameObject);
        }
	}
}
