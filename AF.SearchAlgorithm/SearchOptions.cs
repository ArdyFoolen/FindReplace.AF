using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.SearchAlgorithm
{
    public class SearchOptions
    {
        public static SearchOptions Default { get; set; } = new SearchOptions();

        public bool Overlap { get; set; } = false;
        public bool CaseSensitive { get; set; } = false;
    }
}
