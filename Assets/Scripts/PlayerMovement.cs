using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	
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
//			GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().TakeDamage(1);
            if (moveX != 0 || moveZ != 0)
            {
                pc.curSpeed += .1f * pc.maxSpeed;
                if (pc.curSpeed > pc.maxSpeed)
                {
                    pc.curSpeed = pc.maxSpeed;
                }
                pc.movement.Set(moveX, 0, moveZ);
                Move();
            }
            else
            {
                pc.curSpeed -= .1f * pc.maxSpeed;
                if (pc.curSpeed < 0)
                {
                    pc.curSpeed = 0;
                }
            }

            if (pc.curSpeed == 0)
            {
                pc.stateEnded = true;
            }
            
            // if (pc.rb.velocity.magnitude < .5)
        }


        public override PlayerState HandleInput()
        {
            if (pc.stateEnded && (!Input.GetButton("Vertical") && !Input.GetButton("Vertical")))
            {
				return new Idle(pc);
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