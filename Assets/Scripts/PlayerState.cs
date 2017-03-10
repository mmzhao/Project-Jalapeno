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

}

public enum PlayerStateIndex
{
    IDLE, RUN, DASH, JUMP, IDLE_ATTACK
}