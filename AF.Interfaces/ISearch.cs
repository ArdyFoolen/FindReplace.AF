namespace AF.Interfaces
{
    public interface ISearch
    {
        bool Contains(string source);
        string Replace(string source, string replaceText);
    }
}