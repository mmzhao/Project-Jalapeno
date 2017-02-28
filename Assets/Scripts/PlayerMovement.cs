using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    float h = 0, v = 0;

    void Awake ()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask ("Floor");

        // Set up references.
        anim = GetComponent <Animator> ();
        playerRigidbody = GetComponent <Rigidbody> ();
    }

    void Update ()
    {
        // Store the input axes.
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate ()
    {
        // Move the player around the scene.
        Move (h, v);
    }

    void Move (float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set (h, 0f, v);
            
        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition (transform.position + movement);
    }


}