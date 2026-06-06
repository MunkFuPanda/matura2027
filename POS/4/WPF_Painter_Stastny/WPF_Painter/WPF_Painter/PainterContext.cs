using Painter;

namespace WPF_Painter
{
    internal class PainterContext
    {
        public PainterControl Painter { get; set; }

        public PainterContext(PainterControl painter)
        {
            Painter = painter;
        }
    }
}
