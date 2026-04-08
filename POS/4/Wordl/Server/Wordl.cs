using DataModels;

namespace Server
{
    internal class Wordl
    {
        internal int Tries { get; set; } = 0;
        internal Word word { get; set; }

        internal bool IsGameOver = false;

        internal Wordl(Word word)
        {
            this.word = word;
        }
    }
}
