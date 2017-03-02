using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour {

    public Transform target;
    Camera thisCamera;
    public bool customAngle;
    public Vector3 customCameraAngle;
    Quaternion defaultCameraAngle = Quaternion.Euler(Mathf.Rad2Deg * Mathf.Atan2(4f,3f), 0, 0);
    Quaternion cameraAngle;

    public float maxOrthographicSize = 20;

    Vector3 offset;
    public float smoothing = 5f;

	void Start () {
        thisCamera = gameObject.GetComponent<Camera>();
        thisCamera.orthographic = true;
        if (!customAngle)
        {
            cameraAngle = defaultCameraAngle;
        }
        else
        {
            cameraAngle = Quaternion.Euler(customCameraAngle);
        }
        offset = new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * cameraAngle.eulerAngles.x) * maxOrthographicSize * 2, -Mathf.Cos(Mathf.Deg2Rad * cameraAngle.eulerAngles.x) * maxOrthographicSize * 2);
        transform.position = target.position + offset;
        transform.rotation = cameraAngle;
	}
	
	void FixedUpdate () {
        Vector3 targetCamPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
