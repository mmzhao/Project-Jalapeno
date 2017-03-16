public abstract class PlayerState
{
    protected PlayerController pc;
    protected static readonly int playerState;
    protected static readonly string animState = "State";

    public PlayerState(PlayerController pc)
    {
        this.pc = pc;
    }

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
    public abstract PlayerState HandleInput();
    public virtual void Animate()
    {
        pc.anim.SetFloat("p2mX", pc.playerToMouse.x); //player-to-mouse-X
        pc.anim.SetFloat("p2mZ", pc.playerToMouse.z); //player-to-mouse-Z
    }
}

public enum PlayerStateIndex
{
    IDLE, RUN, DASH, JUMP, IDLE_ATTACK
}