namespace MathEquation.CodeAnalysis.Impl
{
    public class ElementPosition
    {
        public int Start { get; set; }
        public int End { get; set; }
        public ElementPosition(int start, int end)
        {
            End = end;
            Start = start;
        }
    }
}
