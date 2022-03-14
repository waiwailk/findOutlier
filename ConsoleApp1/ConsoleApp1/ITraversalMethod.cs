namespace Methodologies
{
    public interface ITraversalMethod
    {
        void MoveAndCompare();

        // for unit test
        string GetTestResult();
    }
}
