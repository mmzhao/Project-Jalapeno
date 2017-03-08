using UnityEngine;

public class DirectionUtil {
    public float AngleDifference(Direction from, Direction to)
    {
        return Mathf.Abs(from - to) * 45;
    }

    public Direction LeftAdjacent(Direction d)
    {
        return (Direction)(((int)d - 1) % 8);
    }

    public Direction RightAdjacent(Direction d)
    {
        return (Direction)(((int)d + 1) % 8);
    }
}
