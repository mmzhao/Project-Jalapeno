using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	
    public class Idle : PlayerState
    {
        new protected static readonly int playerState = (int) PlayerStateIndex.IDLE;

        public Idle(PlayerController pc) : base(pc)
        {
            this.pc.anim.SetInteger(animState, playerState);
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
            // Check for directional inputs
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal != 0 || vertical != 0)
            {
                // Change the direction we're facing
                pc.facing = DirectionUtil.FloatToDir(vertical, horizontal);
                // End State
                pc.stateEnded = true;
                return;
            }

            // Check for attack inputs
            float attack1 = Input.GetAxisRaw("Attack1");
            float attack2 = Input.GetAxisRaw("Attack2");
            if (attack1 != 0 || attack2 != 0)
            {
                pc.stateEnded = true;
                return;
            }
        }

        public override PlayerState HandleInput()
        {
            if (pc.stateEnded && (Input.GetButton("Attack1") || Input.GetButton("Attack2")))
            {
                if (Input.GetButton("Attack1"))
                {
                    return new Attack1(pc);
                }
                if (Input.GetButton("Attack2"))
                {
                    return new Attack2(pc);
                }
            }
            if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
            {
                if (Input.GetButton("Dash"))
                {
                    return new Dash(pc);
                }
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
        new protected static readonly int playerState = (int)PlayerStateIndex.RUN;

        public Vector3 movement;
        public float curSpeed;
        public float moveX;
        public float moveZ;

        public Running(PlayerController playerController) : base(playerController)
        {
            this.pc.anim.SetInteger(animState, playerState);
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
//          GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().TakeDamage(1);
            if (moveX != 0 || moveZ != 0)
            {
                pc.facing = DirectionUtil.FloatToDir(moveZ, moveX);
                pc.curSpeed += .1f * pc.maxSpeed;
                if (pc.curSpeed > pc.maxSpeed)
                {
                    pc.curSpeed = pc.maxSpeed;
                }
				pc.movement = new Vector3(moveX, 0, moveZ);
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

            if (pc.curSpeed == 0 || Input.GetAxisRaw("Dash") != 0 || Input.GetAxisRaw("Attack1") != 0 || Input.GetAxisRaw("Attack2") != 0)
            {
                pc.stateEnded = true;
            }
            
            // if (pc.rb.velocity.magnitude < .5)
        }


        public override PlayerState HandleInput()
        {
            if (pc.stateEnded && Input.GetButton("Attack1"))
            {
                return new Attack1(pc);
            }
            if (pc.stateEnded && Input.GetButton("Attack2"))
            {
                return new Attack2(pc);
            }
            if (pc.stateEnded && Input.GetButton("Dash"))
            {
                return new Dash(pc);
            }
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

    class Dash : PlayerState
    {
        new protected static readonly int playerState = 2;

        PlayerController pc;
        public Vector3 dir;
        int numMoves = 4;
        int curMoves;

        public Dash(PlayerController playerController) : base(playerController)
        {
            curMoves = 0;
        }

        public override void Enter()
        {
            dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        public override void Exit()
        {

        }

        public override void FixedUpdate()
        {
//          GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().TakeDamage(1);
            
            Move(dir);
            curMoves++;
            if (curMoves == numMoves)
            {
                pc.stateEnded = true;
            }
            
            // if (pc.rb.velocity.magnitude < .5)
        }


        public override PlayerState HandleInput()
        {
            if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Vertical")) )
            {
                return new Running(pc);
            }
            else
            {
                return new Idle(pc);
            }
            // return null;
        }

        public override void Update()
        {

        }

        public void Move (Vector3 dir)
        {
            Vector3 dif = dir.normalized * pc.dashSpeed * Time.deltaTime;
            pc.rb.MovePosition (pc.transform.position + dif);
        }

       
    }
}