﻿using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //    public float speed = 6f;            // The speed that the player will move at.

    //    Vector3 movement;                   // The vector to store the direction of the player's movement.
    //    Animator anim;                      // Reference to the animator component.
    //    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    //    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    //    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    //    float h = 0, v = 0;

    //    void Awake ()
    //    {
    //        // Create a layer mask for the floor layer.
    //        floorMask = LayerMask.GetMask ("Floor");

    //        // Set up references.
    //        anim = GetComponent <Animator> ();
    //        playerRigidbody = GetComponent <Rigidbody> ();
    //    }

    //    void Start ()
    //    {

    //    }

    //    void Update ()
    //    {
    //        // Store the input axes.
    //        h = Input.GetAxisRaw("Horizontal");
    //        v = Input.GetAxisRaw("Vertical");
    //    }

    //    void FixedUpdate ()
    //    {
    //        // Move the player around the scene.
    //        Move (h, v);

    //        // Turn the player to face the mouse cursor.
    ////        Turning ();

    //        // Animate the player.
    ////        Animating (h, v);
    //    }


    //    void Move (float h, float v)
    //    {
    //        // Set the movement vector based on the axis input.
    //        movement.Set (h, 0f, v);

    //        // Normalise the movement vector and make it proportional to the speed per second.
    //        movement = movement.normalized * speed * Time.deltaTime;

    //        // Move the player to it's current position plus the movement.
    //        playerRigidbody.MovePosition (transform.position + movement);
    //    }


    //    void Turning ()
    //    {
    //        // Create a ray from the mouse cursor on screen in the direction of the camera.
    //        Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

    //        // Create a RaycastHit variable to store information about what was hit by the ray.
    //        RaycastHit floorHit;

    //        // Perform the raycast and if it hits something on the floor layer...
    //        if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
    //        {
    //            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
    //            Vector3 playerToMouse = floorHit.point - transform.position;

    //            // Ensure the vector is entirely along the floor plane.
    //            playerToMouse.y = 0f;

    //            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
    //            Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

    //            // Set the player's rotation to this new rotation.
    //            playerRigidbody.MoveRotation (newRotatation);
    //        }

    //        Vector3 turnDir = new Vector3(Input.GetAxisRaw("Mouse X") , 0f , Input.GetAxisRaw("Mouse Y"));

    //        if (turnDir != Vector3.zero)
    //        {
    //            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
    //            Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

    //            // Ensure the vector is entirely along the floor plane.
    //            playerToMouse.y = 0f;

    //            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
    //            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

    //            // Set the player's rotation to this new rotation.
    //            playerRigidbody.MoveRotation(newRotatation);
    //        }
    //    }


    //    void Animating (float h, float v)
    //    {
    //        // Create a boolean that is true if either of the input axes is non-zero.
    //        bool walking = h != 0f || v != 0f;

    //        // Tell the animator whether or not the player is walking.
    //        anim.SetBool ("IsWalking", walking);
    //    }

    public class Idle : PlayerState
    {
        PlayerController pc;

        public Idle(PlayerController playerController)
        {
            this.pc = playerController;
        }

        public override void Enter()
        {
            pc.curSpeed = 0;
        }

        public override void Exit()
        {
            // pc.rb.AddForce(new Vector3(moveX, 0, moveZ));
        }

        public override void FixedUpdate()
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                pc.stateEnded = true;
            }
        }

        public override PlayerState HandleInput()
        {
            if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
            {
                return new Running(pc);
            }
            return null;
        }

        public override void Update()
        {

        }
    }

    class Running : PlayerState
    {
        PlayerController pc;
        public Vector3 movement;
        public float curSpeed;
        public float moveX;
        public float moveZ;

        public Running(PlayerController playerController)
        {
            this.pc = playerController;
            this.movement = playerController.movement;
            this.curSpeed = playerController.curSpeed;
        }

        public override void Enter()
        {
            this.moveX = Input.GetAxisRaw("Horizontal");
            this.moveZ = Input.GetAxisRaw("Vertical");
            // pc.movement.Set(moveX, 0, moveZ);
        }

        public override void Exit()
        {

        }

        public override void FixedUpdate()
        {
            // Debug.Log(pc.maxSpeed);
            // if (pc.rb.velocity.magnitude < pc.maxSpeed)
            // {
            //     pc.rb.AddForce(50 * moveX, 0, 50 * moveZ);
            // }
            // // Vector3 movement = pc.movement.normalized * pc.curSpeed * Time.deltaTime;
            // pc.maxSpeed = 1.0f;
            // float change = .1f;
            // Vector3 inputMovement = new Vector3(this.moveX, 0, this.moveZ).normalized * pc.maxSpeed;
            // pc.movement += (inputMovement - pc.movement) * change;
            // pc.rb.MovePosition (pc.transform.position + pc.movement);
            pc.curSpeed += .1f * pc.maxSpeed;
            if (pc.curSpeed > pc.maxSpeed)
            {
                pc.curSpeed = pc.maxSpeed;
            }
            pc.movement.Set(moveX, 0, moveZ);
            Move();
            // if (pc.rb.velocity.magnitude < .5)
        }


        public override PlayerState HandleInput()
        {
            if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Vertical")))
            {
                return new Running(pc);
            }
            return null;
        }

        public override void Update()
        {
            this.moveX = Input.GetAxisRaw("Horizontal");
            this.moveZ = Input.GetAxisRaw("Vertical");
        }

        public void Move ()
        {
            Vector3 dif = pc.movement.normalized * pc.curSpeed * Time.deltaTime;

            pc.rb.MovePosition (pc.transform.position + dif);
       }

       
    }
}