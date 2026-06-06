using AbcRobotCore;

namespace Robotersteuerung.Models
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public static class DirectionConverter
    {
        public static AbcRobotCore.RobotField.Direction ToAbcDirection(Direction dir)
        {
            return dir switch
            {
                Direction.UP => AbcRobotCore.RobotField.Direction.Up,
                Direction.DOWN => AbcRobotCore.RobotField.Direction.Down,
                Direction.LEFT => AbcRobotCore.RobotField.Direction.Left,
                Direction.RIGHT => AbcRobotCore.RobotField.Direction.Right,
                _ => AbcRobotCore.RobotField.Direction.Up
            };
        }
    }

    public class Robot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public List<char> CollectedLetters { get; set; }

        public Robot(int startX, int startY)
        {
            X = startX;
            Y = startY;
            CollectedLetters = new List<char>();
        }

        public void Move(Direction direction, GameField field)
        {
            int newX = X;
            int newY = Y;

            switch (direction)
            {
                case Direction.UP:
                    newY--;
                    break;
                case Direction.DOWN:
                    newY++;
                    break;
                case Direction.LEFT:
                    newX--;
                    break;
                case Direction.RIGHT:
                    newX++;
                    break;
            }

            // Check bounds
            if (newX >= 0 && newX < field.Width && newY >= 0 && newY < field.Height)
            {
                X = newX;
                Y = newY;
            }
        }

        public char? GetCharInDirection(Direction direction, GameField field)
        {
            int checkX = X;
            int checkY = Y;

            switch (direction)
            {
                case Direction.UP:
                    checkY--;
                    break;
                case Direction.DOWN:
                    checkY++;
                    break;
                case Direction.LEFT:
                    checkX--;
                    break;
                case Direction.RIGHT:
                    checkX++;
                    break;
            }

            if (checkX >= 0 && checkX < field.Width && checkY >= 0 && checkY < field.Height)
            {
                return field.Field[checkY, checkX];
            }

            return null;
        }

        public bool CanMoveInDirection(Direction direction, GameField field)
        {
            var charAtPos = GetCharInDirection(direction, field);
            return charAtPos.HasValue && charAtPos.Value != '#';
        }

        public void Collect(GameField field)
        {
            char cell = field.Field[Y, X];
            if (cell != ' ' && cell != '#')
            {
                CollectedLetters.Add(cell);
                field.Field[Y, X] = ' ';
            }
        }
    }
}
