using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Settings
{
    public class EndpointsSettings
    {
        // Products
        public string GetAllProducts { get; set; } = string.Empty;
        public string GetProductById { get; set; } = string.Empty;
        public string CreateProduct { get; set; } = string.Empty;
        public string ProductPagination { get; set; } = string.Empty;
        public string ProductByTitle { get; set; } = string.Empty;
        public string ProductByPrices { get; set; } = string.Empty;

        // Categories
        public string GetAllCategories { get; set; } = string.Empty;
        public string GetCategoryById { get; set; } = string.Empty;
        public string CreateCategory { get; set; } = string.Empty;
    }
}
