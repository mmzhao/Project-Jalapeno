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

    // Calculates (vertical + 3 * horizontal); outputs the corresponding direction. Assumes not 0 (returns NW by default).
    public static Direction FloatToDir(float vertical, float horizontal)
    {
        float input = vertical + 3 * horizontal;
        if (input == 1)
        {
            return Direction.N;
        }
        else if (input == 4)
        {
            return Direction.NE;
        }
        else if (input == 3)
        {
            return Direction.E;
        }
        else if (input == 2)
        {
            return Direction.SE;
        }
        else if (input == -1)
        {
            return Direction.S;
        }
        else if (input == -4)
        {
            return Direction.SW;
        }
        else if (input == -3)
        {
            return Direction.W;
        }
        else
        {
            return Direction.NW;
        }
    }

    // Returns a unit vector in the input direction
    public static Vector3 DirToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.N:
                return Vector3.Normalize(new Vector3(0, 0, 1));
            case Direction.NE:
                return Vector3.Normalize(new Vector3(1, 0, 1));
            case Direction.E:
                return Vector3.Normalize(new Vector3(1, 0, 0));
            case Direction.SE:
                return Vector3.Normalize(new Vector3(1, 0, -1));
            case Direction.S:
                return Vector3.Normalize(new Vector3(0, 0, -1));
            case Direction.SW:
                return Vector3.Normalize(new Vector3(-1, 0, -1));
            case Direction.W:
                return Vector3.Normalize(new Vector3(-1, 0, 0));
            case Direction.NW:
                return Vector3.Normalize(new Vector3(-1, 0, 1));
        }
        return new Vector3(0, 0, 0);
    }
}
