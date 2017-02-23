using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour {

    public GameObject target;
    Camera thisCamera;
    public Vector3 cameraAngle;
    Vector3 defaultCameraAngle = new Vector3();
	// Use this for initialization
	void Start () {
        thisCamera = gameObject.GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
