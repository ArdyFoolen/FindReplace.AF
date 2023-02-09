using AF.SearchAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.SearchAlgorithmTests
{
    public class SearchHelper
    {
        public static IEnumerable<string> EmptySearchTextCases()
        {
            yield return (string)null;
            yield return "";
        }

        public static IEnumerable<(string SearchText, string Source, int[] SearchIndexes, SearchOptions Options)> FindCases()
        {
            yield return ("abcd", "abcd", new int[] { 0 }, SearchOptions.Default);
            yield return ("abcd", "aabcd", new int[] { 1 }, SearchOptions.Default);
            yield return ("abcd", "aaaaabcd", new int[] { 4 }, SearchOptions.Default);
            yield return ("abcd", "aaaaaaaaabcd", new int[] { 8 }, SearchOptions.Default);
            yield return ("abcd", "eabcd", new int[] { 1 }, SearchOptions.Default);
            yield return ("abcd", "eeeeabcd", new int[] { 4 }, SearchOptions.Default);
            yield return ("abcd", "eeeeeeeeabcd", new int[] { 8 }, SearchOptions.Default);
            yield return ("cdcd", "eecdcdeeabcd", new int[] { 2 }, SearchOptions.Default);
            yield return ("abcd", "abcdabcd", new int[] { 0, 4 }, SearchOptions.Default);
            yield return ("abcd", "abcdaabcd", new int[] { 0, 5 }, SearchOptions.Default);
            yield return ("abcd", "abcdaaaaabcd", new int[] { 0, 8 }, SearchOptions.Default);
            yield return ("abcd", "abcdaaaaaaaaabcd", new int[] { 0, 12 }, SearchOptions.Default);
            yield return ("abcd", "abcdeabcd", new int[] { 0, 5 }, SearchOptions.Default);
            yield return ("abcd", "abcdeeeeabcd", new int[] { 0, 8 }, SearchOptions.Default);
            yield return ("abcd", "abcdeeeeeeeeabcd", new int[] { 0, 12 }, SearchOptions.Default);
            yield return ("cdcd", "cdcdcd", new int[] { 0, 2 }, new SearchOptions() { Overlap = true });

            yield return ("aaba", "babaaaba", new int[] { 4 }, SearchOptions.Default);

            yield return ("ABCD", "abcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "aabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "aaaaabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "aaaaaaaaabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "eabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "eeeeabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "eeeeeeeeabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "eecdcdeeabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "abcdabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "abcdaabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "abcdaaaaabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "abcdaaaaaaaaabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "abcdeabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "abcdeeeeabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("ABCD", "abcdeeeeeeeeabcd", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("CDCD", "cdcdcd", new int[0], new SearchOptions() { CaseSensitive = true, Overlap = true });

            yield return ("abcd", "ABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "AABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "AAAAABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "AAAAAAAAABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "EABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "EEEEABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "EEEEEEEEABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "EECDCDEEABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "ABCDABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "ABCDAABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "ABCDAAAAABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "ABCDAAAAAAAAABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "ABCDEABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "ABCDEEEEABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("abcd", "ABCDEEEEEEEEABCD", new int[0], new SearchOptions() { CaseSensitive = true });
            yield return ("cdcd", "CDCDCD", new int[0], new SearchOptions() { CaseSensitive = true, Overlap = true });

            yield return ("ABCD", "abcd", new int[] { 0 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "aabcd", new int[] { 1 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "aaaaabcd", new int[] { 4 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "aaaaaaaaabcd", new int[] { 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "eabcd", new int[] { 1 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "eeeeabcd", new int[] { 4 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "eeeeeeeeabcd", new int[] { 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "eecdcdeeabcd", new int[] { 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "abcdabcd", new int[] { 0, 4 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "abcdaabcd", new int[] { 0, 5 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "abcdaaaaabcd", new int[] { 0, 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "abcdaaaaaaaaabcd", new int[] { 0, 12 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "abcdeabcd", new int[] { 0, 5 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "abcdeeeeabcd", new int[] { 0, 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("ABCD", "abcdeeeeeeeeabcd", new int[] { 0, 12 }, new SearchOptions() { CaseSensitive = false });
            yield return ("CDCD", "cdcdcd", new int[] { 0, 2 }, new SearchOptions() { CaseSensitive = false, Overlap = true });

            yield return ("abcd", "ABCD", new int[] { 0 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "AABCD", new int[] { 1 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "AAAAABCD", new int[] { 4 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "AAAAAAAAABCD", new int[] { 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "EABCD", new int[] { 1 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "EEEEABCD", new int[] { 4 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "EEEEEEEEABCD", new int[] { 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "EECDCDEEABCD", new int[] { 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "ABCDABCD", new int[] { 0, 4 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "ABCDAABCD", new int[] { 0, 5 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "ABCDAAAAABCD", new int[] { 0, 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "ABCDAAAAAAAAABCD", new int[] { 0, 12 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "ABCDEABCD", new int[] { 0, 5 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "ABCDEEEEABCD", new int[] { 0, 8 }, new SearchOptions() { CaseSensitive = false });
            yield return ("abcd", "ABCDEEEEEEEEABCD", new int[] { 0, 12 }, new SearchOptions() { CaseSensitive = false });
            yield return ("cdcd", "CDCDCD", new int[] { 0, 2 }, new SearchOptions() { CaseSensitive = false, Overlap = true });
        }
    }
}
