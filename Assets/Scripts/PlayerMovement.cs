using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	
    public class Idle : PlayerState
    {
        new public readonly PlayerStateIndex playerState = PlayerStateIndex.IDLE;

        public Idle(PlayerController pc) : base(pc)
        {
            this.pc.anim.SetInteger(animState, (int) playerState);
        }

        public override void Enter()
        {
            pc.rb.velocity = Vector3.zero;
        }

        public override void Exit()
        {
            // pc.rb.AddForce(new Vector3(moveX, 0, moveZ));
        }

        public override void FixedUpdate()
        {
            if (pc.movementInput != Vector3.zero)
            {
                // Change the direction we're facingDirection
                pc.facingDirection = DirectionUtil.FloatToDir(pc.movementInput.z, pc.movementInput.x);
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

//        public override PlayerState HandleInput()
//        {
//			if (pc.stateEnded && ((Input.GetButton("Attack1") && pc.canAttack1()) || (Input.GetButton("Attack2")  && pc.canAttack2())))
//            {
//				if (Input.GetButton("Attack1") && pc.canAttack1())
//                {
//                    return new Attack1(pc);
//                }
//				if (Input.GetButton("Attack2") && pc.canAttack2())
//                {
//                    return new Attack2(pc);
//                }
//            }
//            if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
//            {
//		  		  if (Input.GetButton("Dash") && pc.canDash())
//                {
//					Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
//					return new Dash(pc, dir);
//                }
//                return new Running(pc);
//            }
//            return null;
//        }

        public override void Update()
        {
        }
    }

    public class Running : PlayerState
    {
        new public readonly PlayerStateIndex playerState = PlayerStateIndex.RUN;

        public float curSpeed;
        public float moveX;
        public float moveZ;
        public Vector3 movementVector;
        public float driftingTime = 0;
        public static float DRIFT_TIME = 0.08f; //amount of time to drift after not getting inputs

        public Running(PlayerController playerController) : base(playerController)
        {
            this.pc.anim.SetInteger(animState, (int) playerState);
            this.curSpeed = playerController.curSpeed;
        }

        public override void Enter()
        {
            movementVector = Vector3.zero;
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
//          GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().TakeDamage(1);
            
            if (pc.movementInput != Vector3.zero)
            {
                Direction newDirection = DirectionUtil.FloatToDir(moveZ, moveX);
                movementVector = Vector3.Lerp(movementVector, pc.movementInput * pc.maxSpeed, 20 * Time.deltaTime);
                pc.movingDirection = DirectionUtil.FloatToDir(moveZ, moveX);
                driftingTime = 0;               
            }
            else
            {
                movementVector = Vector3.Lerp(movementVector, Vector3.zero, 20 * Time.deltaTime);
                driftingTime += Time.deltaTime;
            }
            pc.rb.velocity = movementVector;

            if (driftingTime > DRIFT_TIME || Input.GetAxisRaw("Dash") != 0 || Input.GetAxisRaw("Attack1") != 0 || Input.GetAxisRaw("Attack2") != 0)
            {
                pc.stateEnded = true;
            }
            
            // if (pc.rb.velocity.magnitude < .5)
        }

//        public override PlayerState HandleInput()
//        {
//			if (pc.stateEnded && ((Input.GetButton("Attack1") && pc.canAttack1()) || (Input.GetButton("Attack2")  && pc.canAttack2())))
//			{
//				if (Input.GetButton("Attack1") && pc.canAttack1())
//				{
//					return new Attack1(pc);
//				}
//				if (Input.GetButton("Attack2") && pc.canAttack2())
//				{
//					return new Attack2(pc);
//				}
//			}
//			if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
//			{
//				if (Input.GetButton("Dash") && pc.canDash())
//				{
//					Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
//					return new Dash(pc, dir);
//				}
//				return new Running(pc);
//			}
//			return new PlayerMovement.Idle(pc);
//        }

        public override void Update()
        {
            this.moveX = Input.GetAxisRaw("Horizontal");
            this.moveZ = Input.GetAxisRaw("Vertical");
        }

        public override void Animate()
        {
            Vector3 movementDir = movementVector.normalized;
            pc.anim.SetFloat("velocityX", movementDir.x); //player-to-mouse-X
            pc.anim.SetFloat("velocityZ", movementDir.z); //player-to-mouse-Z
        }

    }

    public class Dash : PlayerState
    {
        new public readonly PlayerStateIndex playerState = PlayerStateIndex.DASH;

        public Vector3 dir;
        static float DASH_TIME = 0.25f;
        float dashElapsedTime;

		public Dash(PlayerController playerController, Vector3 dir) : base(playerController)
        {
            dashElapsedTime = 0;
			playerController.dashCharges -= 1;
			this.dir = dir.normalized;

        }

        public override void Enter()
        {

        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            //          GameObject.FindGameObjectWithTag ("Player").GetComponent<Health>().TakeDamage(1);
            pc.rb.velocity = dir * pc.dashSpeed;
            dashElapsedTime += Time.deltaTime;
            if (dashElapsedTime > DASH_TIME)
            {
				pc.nextState = new Running (pc);
                pc.stateEnded = true;
            }
            
            // if (pc.rb.velocity.magnitude < .5)
        }

//		public override PlayerState HandleInput()
//		{
//			if (pc.stateEnded && ((Input.GetButton("Attack1") && pc.canAttack1()) || (Input.GetButton("Attack2")  && pc.canAttack2())))
//			{
//				if (Input.GetButton("Attack1") && pc.canAttack1())
//				{
//					return new Attack1(pc);
//				}
//				if (Input.GetButton("Attack2") && pc.canAttack2())
//				{
//					return new Attack2(pc);
//				}
//			}
//			if (pc.stateEnded && (Input.GetButton("Vertical") || Input.GetButton("Horizontal")))
//			{
//				if (Input.GetButton("Dash") && pc.canDash())
//				{
//					Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
//					return new Dash(pc, dir);
//				}
//				return new Running(pc);
//			}
//			return new PlayerMovement.Idle(pc);
//		}

        public override void Update()
        {

        }

        public override void Animate()
        {
            pc.anim.SetFloat("velocityX", dir.x); //player-to-mouse-X
            pc.anim.SetFloat("velocityZ", dir.z); //player-to-mouse-Z
        }


    }
}