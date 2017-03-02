
public abstract class EnemyState {

    EnemyController ec;

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
    public abstract EnemyState HandleInput();

}
