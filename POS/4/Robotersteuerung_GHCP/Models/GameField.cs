namespace Robotersteuerung.Models
{
    /// <summary>
    /// Represents the game field with letters and obstacles
    /// </summary>
    public class GameField
    {
        public char[,] Field { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }

        public GameField(char[,] field, int startX, int startY)
        {
            Field = field;
            Height = field.GetLength(0);
            Width = field.GetLength(1);
            StartX = startX;
            StartY = startY;
        }
    }
}
