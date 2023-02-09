using AF.Interfaces;

namespace AF.SearchWindows
{
    public class Search : ISearch
    {
        private SearchOptions _options;
        public string SearchText { get; set; }

        private Search(SearchOptions searchOptions)
        {
            _options = searchOptions;
        }

        public static Search Create()
            => Create(SearchOptions.Default);

        public static Search Create(SearchOptions searchOptions)
            => new Search(searchOptions);

        public bool Contains(string source)
        {
            if (_options.CaseSensitive)
            {
                if (source.Contains(SearchText))
                    return true;
            }
            else
                if (source.ToLowerInvariant().Contains(SearchText.ToLowerInvariant()))
                return true;
            return false;
        }

        public string Replace(string source, string replaceText)
            => source.Replace(SearchText, replaceText, !_options.CaseSensitive, null);
    }
}