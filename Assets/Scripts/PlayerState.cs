public abstract class PlayerState
{
    PlayerController pc;

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
    public abstract PlayerState HandleInput();

}