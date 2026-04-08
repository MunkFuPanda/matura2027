namespace Server
{
    public class MSG
    {
        public enum Result { CorrectPosition, WrongPosition, Incorrect}
        public String? Guess { get; set; }
        public List<Result>? Results {  get; set; }
    }
}
