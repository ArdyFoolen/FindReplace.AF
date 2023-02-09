using AF.SearchAlgorithm;
using AF.SearchAlgorithm.Exceptions;

namespace AF.SearchAlgorithmTests
{
    public class SearchTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Find_NoSearch_ShouldThrow()
        {
            // Arrange
            var search = Search.Create();

            // Act & Assert
            var exception = Assert.Throws<SearchTextNullOrEmptyException>(() => search.Find("").ToList());
        }

        [TestCaseSource(typeof(SearchHelper), nameof(SearchHelper.EmptySearchTextCases))]
        public void Find_EmptyCases_ShouldThrow(string criteria)
        {
            // Arrange
            var search = Search.Create();
            search.SearchText = criteria;

            // Act & Assert
            var exception = Assert.Throws<SearchTextNullOrEmptyException>(() => search.Find("").ToList());
        }

        [Test]
        public void Find_SearchToLong_EmtyResult()
        {
            // Arrange
            var search = Search.Create();
            search.SearchText = "abc";

            // Act
            var result = search.Find("").ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.AtMost(0));
        }

        [TestCaseSource(typeof(SearchHelper), nameof(SearchHelper.FindCases))]
        public void Find_SearchTextExists_ShouldFind((
            string SearchText,
            string Source,
            int[] SearchIndexes,
            SearchOptions Options) tuple)
        {
            // Arrange
            var search = Search.Create(tuple.Options);
            search.SearchText = tuple.SearchText;

            // Act
            var result = search.Find(tuple.Source).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(tuple.SearchIndexes.Length));
            Assert.That(result.SequenceEqual(tuple.SearchIndexes));
        }
    }
}