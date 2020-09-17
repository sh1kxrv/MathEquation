namespace MathEquation.CodeAnalysis.Impl
{
    public class ElementPosition
    {
        public int Line { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public ElementPosition(int start, int end, int line)
        {
            Line = line;
            End = end;
            Start = start;
        }
    }
}
