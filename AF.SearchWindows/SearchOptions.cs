using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.SearchWindows
{
    public class SearchOptions
    {
        public static SearchOptions Default { get; set; } = new SearchOptions();

        public bool CaseSensitive { get; set; } = false;
    }
}
