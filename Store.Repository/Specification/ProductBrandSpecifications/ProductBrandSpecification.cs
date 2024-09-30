using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductBrandSpecifications
{
    public class ProductBrandSpecification
    {
        public string? Sort { get; set; }

        public int PageIndex { get; set; } = 1;
        private const int MAXPAGESIZE = 10;
        private int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value>MAXPAGESIZE) ? MAXPAGESIZE : value;
        }


        private string? _search;

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
    }
}
