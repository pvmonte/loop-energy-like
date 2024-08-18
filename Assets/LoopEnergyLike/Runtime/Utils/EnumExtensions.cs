using System;

public static class EnumExtensions
{
    public static SquareNodeDirections GetNext(this SquareNodeDirections currentValue)
    {
        switch (currentValue)
        {
            case SquareNodeDirections.Up:
                return SquareNodeDirections.Right;
            case SquareNodeDirections.Right:
                return SquareNodeDirections.Down;
            case SquareNodeDirections.Down:
                return SquareNodeDirections.Left;
            case SquareNodeDirections.Left:
                return SquareNodeDirections.Up;
            default:
                return SquareNodeDirections.Up;
        }
    }

    public static SquareNodeDirections GetOpposite(this SquareNodeDirections currentValue)
    {
        switch (currentValue)
        {
            case SquareNodeDirections.Up:
                return SquareNodeDirections.Down;
            case SquareNodeDirections.Right:
                return SquareNodeDirections.Left;
            case SquareNodeDirections.Down:
                return SquareNodeDirections.Up;
            case SquareNodeDirections.Left:
                return SquareNodeDirections.Right;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentValue), currentValue, "No next value defined.");
        }
    }
}