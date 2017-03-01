public interface PlayerState
{

    void Enter();
    void FixedUpdate();
    void Update();
    void Exit();
    PlayerState HandleInput();

}