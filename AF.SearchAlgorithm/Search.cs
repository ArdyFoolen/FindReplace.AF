using AF.Interfaces;
using AF.SearchAlgorithm.Exceptions;

namespace AF.SearchAlgorithm
{
    public class Search : ISearch
    {
        private SearchOptions _options;

        public string SearchText { get; set; }
        public int SearchLength { get; private set; }

        private Dictionary<char, int> searchTextDictionary = new Dictionary<char, int>();

        private string _searchText { get; set; }
        private int index { get; set; }
        private int searchIndex { get; set; }

        private Search(SearchOptions searchOptions)
        {
            _options = searchOptions;
        }

        public static Search Create()
            => Create(SearchOptions.Default);

        public static Search Create(SearchOptions searchOptions)
            => new Search(searchOptions);

        public bool Contains(string source)
            => Find(source).Any();

        public string Replace(string source, string replaceText)
            => source.Replace(SearchText, replaceText, !_options.CaseSensitive, null);

        public IEnumerable<int> Find(string source)
        {
            Initialize();
            if (source == null || SearchLength > source.Length)
                return Enumerable.Empty<int>();

            if (!_options.CaseSensitive)
                source = source.ToLowerInvariant();

            return FindEnumerable(source);
        }

        private IEnumerable<int> FindEnumerable(string source)
        {
            while (index < source.Length)
            {
                char @char = source[index - searchIndex];

                if (!InDictionary(@char))
                {
                    index += SearchLength - searchIndex;
                    searchIndex = 0;
                }
                else if (IsLast(@char))
                    foreach (var item in Find())
                        yield return item;
                else
                {
                    index += searchTextDictionary[@char];
                    searchIndex = 0;
                }
            }
        }

        private IEnumerable<int> Find()
        {
            searchIndex += 1;

            if (searchIndex == SearchLength)
            {
                yield return index - searchIndex + 1;
                index += _options.Overlap ? 1 : SearchLength;
                searchIndex = 0;
            }
        }

        private bool IsLast(char @char) => @char == _searchText[SearchLength - searchIndex - 1];
        private bool InDictionary(char @char) => searchTextDictionary.ContainsKey(@char);

        private void Initialize()
        {
            AssertSearch();
            SearchLength = SearchText.Length;
            _searchText = !_options.CaseSensitive ? SearchText.ToLowerInvariant() : SearchText;
            _searchText.Reverse().Skip(1).Select((c, i) => AddToDictionary(c, i + 1)).ToList();
            AddToDictionary(_searchText[SearchLength - 1], SearchLength);

            index = SearchLength - 1;
            searchIndex = 0;
        }

        private void AssertSearch()
        {
            if (string.IsNullOrEmpty(SearchText))
                throw new SearchTextNullOrEmptyException();
        }

        private char AddToDictionary(char @char, int index)
        {
            if (!searchTextDictionary.ContainsKey(@char))
                searchTextDictionary.Add(@char, index);
            return @char;
        }
    }
}