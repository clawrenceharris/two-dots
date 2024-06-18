public interface IComparisonRule<T>
{
    public bool Validate(T a, T b, Board board);
}